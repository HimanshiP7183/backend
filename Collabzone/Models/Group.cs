using System;
using System.Collections.Generic;

namespace Collabzone.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupMembers = new HashSet<GroupMember>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }
}
