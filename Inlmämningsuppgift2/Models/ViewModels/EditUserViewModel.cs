using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Inlmämningsuppgift2.Models.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
