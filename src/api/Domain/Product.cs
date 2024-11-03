using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Domain
{
    public class Product
    {
        // Private setter enforces creation through constructor
        public string Id { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Empty constructor for MongoDB serialization
        private Product() { }

        public Product(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Product name cannot be empty");

            Name = name;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Method to update the product
        public void Update(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Product name cannot be empty");

            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void SetId(string id)
        {
            if (!string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("Cannot change existing ID");
                
            Id = id;
        }

        // Factory method for creating new products
        public static Product Create(string name)
        {
            return new Product(name);
        }
    }
}