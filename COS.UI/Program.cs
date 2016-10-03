using COS.DataLayer;
using COS.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COS.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<COSModelContext>());
            //InsertCustomer();
            //InsertMultipleCustomers();
            SimpleCustomerQueries();
            //SimpleCustomerGraphQuery();
            //QueryAndUpdateCustomer();
            //DeleteCustomer();
            Console.ReadKey();
        }

        private static void SimpleCustomerQueries()
        {
            using (var context = new COSModelContext())
            {
                //TO GET A LIST OF CUSTOMERS
                //var customers = context.Customers.ToList();
                //var query = context.Customers;
                //var allCustomers = query.ToList();
                //foreach (var customer in query)
                //foreach (var customer in context.Customers)
                //{
                //    Console.WriteLine(customer.FirstName);
                //}
                //TO FILTER A PARTICULAR CUSTOMER
                var customers = context.Customers.
                    //Where(c => c.LastName == "Igoor");
                    Where(c => c.DateOfBirth >= new DateTime(2000, 01, 01))
                    .ToList();
                //.FirstOrDefault();
                //Console.WriteLine(customers.FirstName);
                foreach (var customer in customers)
                {
                    Console.WriteLine(customer.FirstName);
                }
            }
        }

        private static void InsertMultipleCustomers()
        {
            var customer1 = new Customer
            {
                FirstName = "Shanaya",
                LastName = "Igoor",
                DateOfBirth = new DateTime(2014, 05, 16)
            };
            var customer2 = new Customer
            {
                FirstName = "Preeti",
                LastName = "Halli",
                DateOfBirth = new DateTime(1983, 02, 03)
            };

            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Customers.AddRange(new List<Customer> { customer1, customer2 });
                context.SaveChanges();
            }
        }

        private static void InsertCustomer()
        {
            var customer = new Customer
            {
                FirstName = "Arya",
                LastName = "Igoor",
                DateOfBirth = new DateTime(2010, 12, 24)
            };

            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Customers.Add(customer);
                context.SaveChanges();
            }
        }
    }
}
