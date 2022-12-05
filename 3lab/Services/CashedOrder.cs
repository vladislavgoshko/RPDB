using Microsoft.Extensions.Caching.Memory;
using WebApplicationSewingCompany.Models;

namespace WebApplicationSewingCompany.Services
{
    public class CashedOrder
    {
        private readonly SewingCompanyContext _context;
        private readonly IMemoryCache _cache;
        private double time = 2 * 6 + 240;
        public CashedOrder(SewingCompanyContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddOrders(string cacheKey, int rowNumber)
        {
            IEnumerable<Order> storages = getOrders(rowNumber).ToList();
            if (storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(time)
                });
            }
        }

        public IEnumerable<Order> GetOrders(int rowNumber)
        {
            return getOrders(rowNumber).ToList();
        }

        public IEnumerable<Order> GetOrders(string cacheKey, int rowNumber)
        {
            IEnumerable<Order> storages;
            if (!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = getOrders(rowNumber);
                if (storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(time)));
                }
            }
            
            return storages;
        }
        private IEnumerable<Order> getOrders(int rowNumber)
        {
            var orders = _context.Orders.Take(rowNumber).ToList();
            foreach (Order ord in orders)
            {
                ord.Product = _context.Products.Where(x => x.Id == ord.ProductId).First();
                ord.Customer = _context.Customers.Where(x => x.Id == ord.CustomerId).First();
            }
            return orders;
        }
        public string GetTable(IEnumerable<Order> orders)
        {
            string HtmlString = "<html><head><title>Main</title>" +
                    "<style> " +
                    "\r\n.menu { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                    "\r\np { margin: 0;}" +
                    "\r\n</style></head>" +
                    "<body><div class='menu'>" +
                        "<p><a href = '/'>To main</a></p><br>" +
                        "<p>List of orders:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Product name</th>" +
                        "<th>Customer name</th>" +
                        "<th>Amount</th>" +
                        "<th>Order date</th>" +
                        "<th>Delivery date</th>" +
                        "</tr>";
            foreach (var order in orders)
            {
                HtmlString += "<tr>" +
                $"<td>{order.Id}</td>" +
                $"<td>{order.Product.Name}</td>" +
                $"<td>{order.Customer.Name}</td>" +
                $"<td>{order.Amount}</td>" +
                $"<td>{order.OrderDate.ToString().Split()[0]}</td>" +
                $"<td>{order.DeliveryOrderDate.ToString().Split()[0]}</td>" +
                "</tr>";
            }
            HtmlString += "</table></div></body></html>";
            return HtmlString;
        }

        public string GetTable(string id)
        {
            var orders = _context.Orders.ToList().Where(x => x.Id == Convert.ToInt64(id));
            foreach (Order ord in orders)
            {
                ord.Product = _context.Products.Where(x => x.Id == ord.ProductId).First();
                ord.Customer = _context.Customers.Where(x => x.Id == ord.CustomerId).First();
            }
            return GetTable(orders);
        }
    }
}
