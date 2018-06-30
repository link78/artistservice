using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
   public class AppUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuperUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
