using System;
using System.Collections.Generic;

namespace Collabzone.Models
{
    public partial class User
    {
        public User()
        {
            GroupMembers = new HashSet<GroupMember>();
            Groups = new HashSet<Group>();
        }

        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string AdharCard { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
