using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Models.ViewModels
{
    public class ManageViewModel
    {
        public Customer Customer { get; set; }

        public bool IsAdmin { get; set; }

        [UIHint("password")]
        [DisplayName("Nuvarande Lösenord")]
        public string Password { get; set; }

        [UIHint("password")]
        [DisplayName("Nya Lösenord")]
        public string NewPassword { get; set; }
    }
}
