using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using _2lab;
using _2lab.Models;
using System.Security.AccessControl;

namespace HelloApp
{
    public class Program
    {
        public static void  Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()); // установка пути к текущему каталогу
            builder.AddJsonFile(@"C:\Users\ferru\OneDrive\Документы\5semester\ПриложенияБД\2lab\2lab\appsettings.json"); // получаем конфигурацию из файла appsettings.json
            var config = builder.Build();// создаем конфигурацию
            string connectionString = config.GetConnectionString("DefaultConnection");// получаем строку подключения
            var optionsBuilder = new DbContextOptionsBuilder<SewingCompanyContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;


            Console.WriteLine("Задание 1:");
            Task1();
            Console.WriteLine("Задание 2:");
            Task2();
            Console.WriteLine("Задание 3:");
            Task3();
            Console.WriteLine("Задание 4:");
            Task4();
            Console.WriteLine("Задание 5:");
            Task5();
            Console.WriteLine("Задание 6:");
            Task6();
            Console.WriteLine("Задание 7:");
            Task7();
            Console.WriteLine("Задание 8:");
            Task8();
            Console.WriteLine("Задание 9:");
            Task9();
            Console.WriteLine("Задание 10:");
            Task10();


            Console.Read();
        }
        /// <summary>
        /// Задание 1: Выборка данных на стороне отношения "один"
        /// </summary>
        public static void Task1()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var workers = db.Workers.ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(workers[i]);
                }
            }
        }
        /// <summary>
        /// Задание 2: Выборка данных на стороне отношения "один" и фильтариция данных по двум полям
        /// </summary>
        public static void Task2()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var workers = db.Workers.Where(w => w.Id > 1234 && w.Orders.Count > 2).ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(workers[i]);
                }
            }
        }
        /// <summary>
        /// Задание 3: Выборка данных на стороне отношения "многие" и группировка данных
        /// </summary>
        public static void Task3()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var materials = (from m in db.MaterialLists
                                 group m by m.ProductId into n
                                 orderby n.Key
                                 select new
                                 {
                                     ProductId = n.Key,
                                     MaterialsCount = n.Count()
                                 }
                          ).ToList();
                int i = 0;
                if (materials != null)
                    foreach (var m in materials)
                    {
                        Console.WriteLine(m);
                        if (i++ > 8) break;
                    }
                else
                    Console.WriteLine("pizdec");
            }
        }
        /// <summary>
        /// Задание 4: Выборка данных из двух таблиц и группировка данных
        /// </summary>
        public static void Task4()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var OrdersAndWorkers = (from ord in db.Orders
                                        join w in db.Workers
                                        on ord.WorkerId equals w.Id
                                        orderby w.Id
                                        select new
                                        {
                                            OrderId = ord.Id,
                                            WorkerName = w.Name
                                        }

                          );
                int i = 0;
                foreach (var m in OrdersAndWorkers)
                {
                    Console.WriteLine(m);
                    if (i++ > 8) break;
                }
            }
        }
        /// <summary>
        /// Задание 5: Выборка данных из двух таблиц и фильтрация данных
        /// </summary>
        public static void Task5()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var OrdersAndWorkers = (from ord in db.Orders
                                        join w in db.Workers
                                        on ord.WorkerId equals w.Id
                                        where ord.ImplementationDate > ord.DeliveryOrderDate && w.Orders.Count() == 1
                                        orderby ord.Id
                                        select new
                                        {
                                            OrderId = ord.Id,
                                            WorkerName = w.Name
                                        }
                          );
                int i = 0;
                foreach (var m in OrdersAndWorkers)
                {
                    Console.WriteLine(m);
                    if (i++ > 8) break;
                }
            }
        }
        /// <summary>
        /// Задание 6: Вставка данных в таблицу, стоящей на стороне отношения "один"
        /// </summary>
        public static void Task6()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                Worker w = new Worker()
                {
                    Name = "worker" + db.Workers.Count(),
                    Section = "section" + (new Random()).Next(1, 1000),
                    Position = "position" + (new Random()).Next(1, 1000)
                };
                db.Workers.Add(w);
                db.SaveChanges();
                var workers = db.Workers.OrderByDescending(w => w.Id).ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(workers[i]);
                }
            }
        }
        /// <summary>
        /// Задание 7: Вставка данных в таблицу, стоящей на стороне отношения "многие"
        /// </summary>
        public static void Task7()
        {
            Random random = new Random();
            DateTime date1 = DateTime.Now.AddDays(random.Next(-2000, 100));
            DateTime date2 = date1.AddDays(random.Next(1, 10));
            DateTime date3 = date2.AddDays(random.Next(1, 10));
            DateTime date4 = date3.AddDays(random.Next(1, 10)); using (SewingCompanyContext db = new SewingCompanyContext())
            {
                Order ord = new Order()
                {
                    CustomerId = random.Next(1, db.Customers.Count()),
                    ProductId = random.Next(1, db.Products.Count()),
                    Amount = random.Next(1, 20),
                    OrderDate = date1,
                    ExecutionStartDate = date2,
                    ImplementationDate = date3,
                    DeliveryOrderDate = date4,
                    WorkerId = random.Next(1, db.Workers.Count()),
                };
                db.Orders.Add(ord);
                db.SaveChanges();
                var orders = db.Orders.OrderByDescending(ord => ord.Id).ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(orders[i].ToString());
                }
            }
        }
        /// <summary>
        /// Задание 8: Удаление данных из таблицы, стоящей на стороне отношения «один»
        /// </summary>
        public static void Task8()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var workers = db.Workers.Where(w => w.Id > 29999);
                if (workers.Count() > 0)
                {
                    //db.Workers.RemoveRange(workers);
                    db.SaveChanges();
                }
                var _workers = (from w in db.Workers
                                orderby w.Id descending
                                select w).ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(_workers[i].ToString());
                }
            }
        }
        /// <summary>
        /// Задание 9: Удаление данных из таблицы, стоящей на стороне отношения «многие»
        /// </summary>
        public static void Task9()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var orders = db.Orders.Where(ord => ord.Id > 29999);
                db.Orders.RemoveRange(orders);
                db.SaveChanges();
                var _orders = (from ord in db.Orders
                               orderby ord.Id descending
                               select ord).ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(_orders[i].ToString());
                }
            }
        }
        /// <summary>
        /// Задание 10: Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных
        /// </summary>
        public static void Task10()
        {
            using (SewingCompanyContext db = new SewingCompanyContext())
            {
                var workers = db.Workers.Where(w => w.Id > 29995 && w.Id < 29999);
                foreach (var worker in workers)
                {
                    worker.Position = "MainWorker";
                }
                db.SaveChanges();
                var _workers = (from w in db.Workers
                                orderby w.Id descending
                                select w).ToList();
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(_workers[i].ToString());
                }
            }
        }
    }
}