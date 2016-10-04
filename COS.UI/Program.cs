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
            //SimpleCustomerQueries();
            //SimpleCustomerGraphQuery();
            //QueryAndUpdateCustomer();
            //DeleteCustomer();
            //RetrieveDataWithFind();
            //RetrieveDataWithStoredProc();
            DeleteCustomerWithStoredProc();
            Console.ReadKey();
        }

        private static void DeleteCustomerWithStoredProc()
        {
            var keyVal = 6;
            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("exec DeleteCustomerById {0}", keyVal);
            }
        }

        private static void DeleteCustomer()
        {
            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                var customer = context.Customers.OrderByDescending(o=>o.CustomerId).FirstOrDefault();
                context.Customers.Remove(customer);
                //context.Entry(customer).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void RetrieveDataWithStoredProc()
        {
            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                var customers = context.Customers.SqlQuery("exec GetAllCustomers");
                foreach (var customer in customers)
                {
                    Console.WriteLine(customer.FirstName);
                }
            }
        }

        private static void RetrieveDataWithFind()
        {
            var keyVal = 2;
            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                var customer = context.Customers.Find(keyVal);
                Console.WriteLine("After Find #1: " + customer.FirstName);

                //Second time EF just pulls the record from in-memory
                var anotherCustomer = context.Customers.Find(keyVal);
                Console.WriteLine("After Find #2: " + anotherCustomer.FirstName);
                customer = null;
            }
        }

        private static void QueryAndUpdateCustomer()
        {
            using (var context = new COSModelContext())
            {
                context.Database.Log = Console.WriteLine;
                var customer = context.Customers.FirstOrDefault();
                customer.DateOfBirth = new DateTime(1977, 11, 24);
                context.SaveChanges();
            }
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
