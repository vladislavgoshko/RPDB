using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab4.DbModels;

namespace lab4.Controllers
{
    public class WorkersController : Controller
    {
        private readonly SewingCompanyContext _context;

        public WorkersController(SewingCompanyContext context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 2 * 6 + 240)]
        public IActionResult Index()
        {
              return View(_context.Workers.OrderByDescending(x => x.Id).Take(20).Include(x => x.Orders));
        }

    }
}
