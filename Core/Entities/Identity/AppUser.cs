using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual Address Address { get; set; }
    }
}