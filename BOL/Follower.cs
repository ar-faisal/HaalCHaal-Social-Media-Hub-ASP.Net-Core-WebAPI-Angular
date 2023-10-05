using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Follower
    {
        

        // Add any additional properties you need for the follower, e.g., follower's user ID.
        public string FollowerUserId { get; set; } // User who is following someone.
        public string FollowingUserId { get; set; } // User who is being followed.

        // Navigation properties for users.
        public HCUser FollowerUser { get; set; }
        public HCUser FollowingUser { get; set; }

        
    }
}