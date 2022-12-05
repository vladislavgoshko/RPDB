using Microsoft.Extensions.Caching.Memory;
using WebApplicationSewingCompany.Models;

namespace WebApplicationSewingCompany.Services
{
    public class CashedCustomer
    {
        private readonly SewingCompanyContext _context;
        private readonly IMemoryCache _cache;
        private double time = 2 * 6 + 240;
        public CashedCustomer(SewingCompanyContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddCustomers(string cacheKey, int rowNumber)
        {
            IEnumerable<Customer> storages = getCustomers(rowNumber).ToList();
            if (storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(time)
                });
            }
        }

        public IEnumerable<Customer> GetCustomers(int rowNumber)
        {
            return getCustomers(rowNumber).ToList();
        }

        public IEnumerable<Customer> GetCustomers(string cacheKey, int rowNumber)
        {
            IEnumerable<Customer> storages;
            if (!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = getCustomers(rowNumber).ToList();
                if (storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(time)));
                }
            }
            return storages;
        }

        private string GetTable(IEnumerable<Customer> customers)
        {
            string HtmlString = "<html><head><title>Main</title>" +
                    "<style> " +
                    "\r\n.menu { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                    "\r\np { margin: 0;}" +
                    "\r\n</style></head>" +
                    "<body><div class='menu'>" +
                        "<p><a href = '/'>To main</a></p><br>" +
                        "<p>List of customers:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Name</th>" +
                        "<th>Orders</th>" +
                        "</tr>";
            foreach (var customer in customers)
            {
                HtmlString += "<tr>" +
                $"<td>{customer.Id}</td>" +
                $"<td>{customer.Name}</td>";
                string orders = string.Join("", customer.Orders.Select(x => "<option>Id: " + x.Id.ToString() + "</option>").ToArray());
                HtmlString += $"<td>{(orders == "" ? "No" : $"<select>{orders}</select>")}</td>" +
                "</tr>";
            }
            HtmlString += "</table></div></body></html>";
            return HtmlString;
        }

        public string GetTable(string name)
        {
            var customers = _context.Customers.ToList().Where(x => x.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
            foreach (var customer in customers)
            {
                customer.Orders = _context.Orders.Where(x => x.CustomerId == customer.Id).ToList();
            }
            return GetTable(customers);
        }
        private IEnumerable<Customer> getCustomers(int rowNumber)
        {
            var customers = _context.Customers.Take(rowNumber).ToList();
            foreach (Customer c in customers)
            {
                c.Orders = _context.Orders.Where(x => x.CustomerId == c.Id).ToList();
            }
            return customers;
        }
    }
}
