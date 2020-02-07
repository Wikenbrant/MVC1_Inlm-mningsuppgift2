using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        [Required]
        [DisplayName("Namn")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Address")]
        public string Adress { get; set; }
        [Required]
        [DisplayName("Postnummer")]
        public string ZipCode { get; set; }
        [Required]
        [DisplayName("Postort")]
        public string City { get; set; }
        [Required]
        [DisplayName("Telefonnummer")]
        public string Phone { get; set; }

        [Required]
        [UIHint("email")]
        public string Email { get; set; }

        //public string Username { get; set; }
        //public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
