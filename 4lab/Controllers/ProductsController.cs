using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab4.DbModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace lab4.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SewingCompanyContext _context;

        public ProductsController(SewingCompanyContext context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 2 * 6 + 240)]
        public IActionResult Index()
        {
            var products = _context.Products.OrderByDescending(x => x.Id).Take(20).Include(x => x.MaterialLists);
            (from m in _context.MaterialLists
                join p in products
                on m.ProductId equals p.Id
                select m).Include(x => x.Material).Load();
            return View(products);
        }

    }
}
