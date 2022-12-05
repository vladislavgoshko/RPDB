using lab4.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab4.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SewingCompanyContext _context;
        public OrdersController(SewingCompanyContext context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 2 * 6 + 240)]
        public IActionResult Index()
        {
            return View(_context.Orders.OrderByDescending(x => x.Id).Take(20).Include(c => c.Customer).Include(c => c.Worker).Include(c => c.Product));
        }
    }
}
