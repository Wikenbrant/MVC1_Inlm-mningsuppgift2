using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Models.Entities;

namespace Inlmämningsuppgift2.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string ReturnUrl { get; set; } = "/";

        public Customer Customer { get; set; }

        [Required]
        [UIHint("password")]
        [DisplayName("Lösenord")]
        public string Password { get; set; }
    }
}
