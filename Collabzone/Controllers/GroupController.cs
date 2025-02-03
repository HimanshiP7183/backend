using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Collabzone.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Collabzone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly CollabZoneContext _context;

        public GroupController(CollabZoneContext context)
        {
            _context = context;
        }

        // POST: api/Group/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateGroup([FromBody] Group newGroup)
        {
            if (newGroup == null)
            {
                return BadRequest("The newGroup field is required.");
            }

            // Check if the user exists
            var user = await _context.Users.FindAsync(newGroup.CreatedBy);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Ensure the user has not created more than 5 groups
            if (_context.Groups.Count(g => g.CreatedBy == newGroup.CreatedBy) >= 5)
            {
                return BadRequest("User cannot create more than 5 groups.");
            }

            // Set creation and update timestamps
            newGroup.CreatedAt = DateTime.Now;
            newGroup.UpdatedAt = DateTime.Now;

            // Add the new group to the database
            _context.Groups.Add(newGroup);
            await _context.SaveChangesAsync();

            // After saving the group, set group ID correctly
            newGroup.GroupId = _context.Groups.OrderByDescending(g => g.GroupId).FirstOrDefault()?.GroupId ?? 0;

            // Add the user as an admin by default
            var groupMember = new GroupMember
            {
                GroupId = newGroup.GroupId,
                UserId = newGroup.CreatedBy,
                IsAdmin = true,
                JoinedAt = DateTime.Now
            };

            // Add the user as a group member (with admin rights)
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();

            // Return the created group
            return Ok(newGroup);
        }

        // GET: api/Group/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _context.Groups
                .Include(g => g.GroupMembers)
                .ThenInclude(gm => gm.User)
                .FirstOrDefaultAsync(g => g.GroupId == id);

            if (group == null)
            {
                return NotFound("Group not found.");
            }

            return Ok(group);
        }

        // PUT: api/Group/Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateGroup([FromBody] Group updatedGroup)
        {
            if (updatedGroup == null)
            {
                return BadRequest("Invalid group data.");
            }

            var existingGroup = await _context.Groups.FindAsync(updatedGroup.GroupId);
            if (existingGroup == null)
            {
                return NotFound("Group not found.");
            }

            // Only admins can update group info (example: group name)
            var groupMember = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == updatedGroup.GroupId && gm.UserId == updatedGroup.CreatedBy);
            if (groupMember == null || !groupMember.IsAdmin)
            {
                return Unauthorized("Only admins can update the group.");
            }

            existingGroup.GroupName = updatedGroup.GroupName;
            existingGroup.UpdatedAt = DateTime.Now;

            _context.Groups.Update(existingGroup);
            await _context.SaveChangesAsync();

            return Ok(existingGroup);
        }

        // DELETE: api/Group/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteGroup(int id, [FromQuery] int userId)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            // Check if the user trying to delete is an admin
            var groupMember = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == id && gm.UserId == userId);
            if (groupMember == null || !groupMember.IsAdmin)
            {
                return Unauthorized("Only admins can delete the group.");
            }

            // Remove the group from the database
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return Ok("Group deleted successfully.");
        }
    }
}
