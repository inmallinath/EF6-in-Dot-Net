using COS.DataLayer;
using COS.DomainClasses;
using COS.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
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
            //DeleteCustomerWithStoredProc();
            //InsertCustomerWithOrder();
            //EagerLoading();
            //LoadWithProjection();
            //ExplicitLoading();
            LazyLoading();

            Console.ReadKey();
        }

        //WHEN QUERYING
        //Eager Loading - uses the include method

        private static void EagerLoading()
        {
            using (var context = new COSModelContext())
            {
                //for help with lambda intellisense include using system.data.entity
                //var eagerLoadGraph1 = context.Customers.Include(c=>c.Orders).ToList();
                //var eagerLoadGraph2 = context.Customers.Include("Orders").ToList();
                //var eagerLoadGraph3 = context.Customers.Include("Orders.LineItems").ToList();

                var eagerLoadGraph4 = context.Customers
                        .Where(c => c.Orders.Any())
                        .Include(c => c.Orders.Select(o => o.LineItems.Select(l => l.Product)))
                        .ToList();

                var customer = eagerLoadGraph4[0];
            }
        }

        //Projection - Specify the navigation properties and sort your results
        private static void LoadWithProjection()
        {
            using (var context = new COSModelContext())
            {
                var customerOrderGraph = context.Customers
                    .Select(c => new { c, c.Orders })
                    .ToList();
                var customer = customerOrderGraph[2].c;

                var customerWithFirstOrder =
                    context.Customers
                    .Select(c => new
                    {
                        c,
                        FirstOrder = c.Orders.OrderBy(o => o.OrderDate).FirstOrDefault()
                    })
                    .ToList();
            }
        }

        //AFTER THE FACT
        //Explicit Loading - Explicitly by load method
        private static void ExplicitLoading()
        {
            using (var context = new COSModelContext())
            {
                var customer = context.Customers.OrderByDescending(c=>c.CustomerId).First();
                context.Entry(customer).Collection(c => c.Orders).Load();
                Console.WriteLine("Order Count for {0} : {1}", customer.FirstName, customer.Orders.Count);
            }
        }

        //Lazy Loading - implicitly using the virtual keyword
        private static void LazyLoading()
        {
            using (var context = new COSModelContext())
            {
                //context.Configuration.LazyLoadingEnabled = true;
                var customer = context.Customers.OrderByDescending(c => c.CustomerId).First();
                //context.Entry(customer).Collection(c => c.Orders).Load();
                Console.WriteLine("Order Count for {0} : {1}", customer.FirstName, customer.Orders.Count);
            }
        }

        private static void InsertCustomerWithOrder()
        {
            var products = GetProducts();
            var product1 = products[2];
            var product2 = products[3];
            var customer = new Customer
            {
                FirstName = "Divya",
                LastName = "Igoor",
                ContactDetail = new ContactDetail
                {
                    TwitterAlias = "divdeepmys"
                },
                DateOfBirth = new DateTime(1974, 07, 20)
            };
            var order = new Order
            {
                DestinationLatLong = DbGeography.FromText("POINT(12.972442 77.580643)"),
                OrderDate = DateTime.Now,
                OrderSource = OrderSource.InPerson,
                LineItems = { new LineItem { ProductId = product1.ProductId, Quantity = 4},
                              new LineItem { ProductId = product2.ProductId, Quantity = 6}
                            }
            };
            customer.Orders.Add(order);
            using (var context = new COSModelContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
    }

        private static List<Product> GetProducts()
        {
            using (var context = new COSModelContext())
            {
                return context.Products.ToList();
            }
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
