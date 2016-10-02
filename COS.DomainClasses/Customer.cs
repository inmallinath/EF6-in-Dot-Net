using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COS.DomainClasses
{
    public class Customer
    {
        private ICollection<Address> _addresses;
        private ICollection<Order> _orders;

        public Customer()
        {
            _orders = new List<Order>();
            _addresses = new List<Address>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ContactDetail ContactDetail { get; set; }

        public virtual ICollection<Order> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
        public virtual ICollection<Address> Addresses
        {
            get { return _addresses; }
            set { _addresses = value; }
        }
    }
}
