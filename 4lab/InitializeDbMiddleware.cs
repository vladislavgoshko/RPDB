using lab4.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace lab4
{
    public class InitializeDbMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SewingCompanyContext dbContext;

        public InitializeDbMiddleware(RequestDelegate next, SewingCompanyContext context)
        {
            this._next = next;
            this.dbContext = context;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await dbContext.Workers.AddAsync(new Worker()
            {
                Name = "Worker" + DateTime.Now.Ticks,
                Section = "Section" + DateTime.Now.Ticks,
                Position = "Position" + DateTime.Now.Ticks
            });
            await dbContext.Products.AddAsync(new Product()
            {
                Name = "Product" + DateTime.Now.Ticks,
                Price = (decimal)new Random().NextDouble()*100 + 10,
            });
            var d1 = DateTime.Now.AddDays(-new Random().Next(100, 500));
            var d2 = d1.AddDays(new Random().Next(1, 10));
            var d3 = d2.AddDays(new Random().Next(1, 10));
            var d4  = d3.AddDays(new Random().Next(1, 10));
            await dbContext.Orders.AddAsync(new Order()
            {
                CustomerId = new Random().Next(0, dbContext.Customers.OrderByDescending(x => x.Id).First().Id),
                ProductId = new Random().Next(0, dbContext.Products.OrderByDescending(x => x.Id).First().Id),
                WorkerId = new Random().Next(0, dbContext.Workers.OrderByDescending(x => x.Id).First().Id),
                Amount = new Random().Next(1, 10),
                OrderDate = d1,
                ExecutionStartDate = d2,
                ImplementationDate = d3,
                DeliveryOrderDate = d4

            });
            await dbContext.SaveChangesAsync();
            await _next.Invoke(context);
        }
    }
}
