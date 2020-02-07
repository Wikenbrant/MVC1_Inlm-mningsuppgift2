using System.Collections.Generic;

namespace Inlmämningsuppgift2.Models.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        //public string Username { get; set; }
        //public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
