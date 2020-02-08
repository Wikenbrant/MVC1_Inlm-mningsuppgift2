using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Inlmämningsuppgift2.Models.User
{
    public enum CustomerType
    {
        None,
        Regular,
        Premium
    }
    public class ApplicationUser : IdentityUser
    {
        public CustomerType CustomerType { get; set; }
        public int BonusPoints { get; set; }
        public int CustomerId { get; set; }
    }
}
