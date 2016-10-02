using System.Collections.Generic;

namespace COS.DomainClasses
{
    public class Category
    {
        private ICollection<Product> _products;

        public Category()
        {
            _products = new List<Product>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products
        {
            get { return _products; }
            set { _products = value; }
        }
    }
}