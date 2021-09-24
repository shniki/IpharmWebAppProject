using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IpharmWebAppProject.Data;
using IpharmWebAppProject.Models;
using System.Collections.ObjectModel;

namespace IpharmWebAppProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IpharmContext _context;
        private List<string> brands; //create brand page
        private List<List<string>> types; //create type page, create product -> category -> type

        public ProductsController(IpharmContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Where(p => p.Active == true).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(r => r.Reviews)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Price,Amount,Gender,Category,Type,Brand,Description,Rate,PicUrl1,PicUrl2,PicUrl3,Stock,Active")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Price,Amount,Gender,Category,Type,Brand,Description,Rate,PicUrl1,PicUrl2,PicUrl3,Stock,Active")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public async Task<IActionResult> Search(string query)
        {
            if (query == null)
            {
                return NotFound();
            }
            var products = await _context.Products.Where(c => c.Name.Contains(query)).ToListAsync();
            return View(products);
        }



        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        [HttpGet]
        public ActionResult Statistics()
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Customer") //logged in as customer
                return NotFound();

            //statistic 1 - profit each month
            ICollection<Stat> statistic1 = new Collection<Stat>();
            var res1 = from o in _context.Orders
                       where o.Status != Status.Cart & ((o.OrderDate.Month <= DateTime.Today.Month & o.OrderDate.Year == DateTime.Today.Year)
                       | (o.OrderDate.Month > DateTime.Today.Month & o.OrderDate.AddYears(1).Year == DateTime.Today.Year))
                       group o by o.OrderDate.Month into mo
                       select new { Key = mo.Key, Profit = mo.Sum(mo => mo.Price) };

            foreach (var v in res1)
            {
                statistic1.Add(new Stat(v.Key.ToString(), v.Profit));
            }
            ViewBag.stat1 = statistic1;

            //statistic 2 - most bought products
            ICollection<Stat> statistic2 = new Collection<Stat>();
            var yearorders2 = from o in _context.Orders
                              where o.Status != Status.Cart & (o.OrderDate.Month <= DateTime.Today.Month & o.OrderDate.Year == DateTime.Today.Year)
                              | (o.OrderDate.Month > DateTime.Today.Month & o.OrderDate.AddYears(1).Year == DateTime.Today.Year)
                              select new { key = o.OrderID }; //orders from the past year
            var sumpro2 = from proinord in _context.ProductInOrders
                          join pro in yearorders2 on proinord.OrderID equals pro.key
                          group proinord by proinord.ProductID into products2
                          select new { Key = products2.Key, Amount = products2.Sum(products2 => products2.Amount) };
            //amount bought of each product (in past year)
            var maxpro2 = (from maxP in _context.Products
                           join noy in sumpro2 on maxP.ProductID equals noy.Key
                           orderby noy.Amount descending
                           select new
                           {
                               Name = maxP.Name,
                               Amount = noy.Amount
                           }).Take(5);
            //add name of product & take top 5

            foreach (var v in maxpro2)
            {
                statistic2.Add(new Stat(v.Name.ToString(), v.Amount));
            }
            ViewBag.stat2 = statistic2;

            //statistic 3 - most active users
            ICollection<Stat> statistic3 = new Collection<Stat>();
            var res3 = (from ord in _context.Orders
                        where ord.Status != Status.Cart
                        group ord by ord.Email into cusord
                        select new { Email = cusord.Key, Orders = cusord.Count() }).OrderByDescending(noy => noy.Orders).Take(5);

            foreach (var v in res3)
            {
                statistic3.Add(new Stat(v.Email.ToString(), v.Orders));
            }
            ViewBag.stat3 = statistic3;

            //statistic 4 - most bought categories
            ICollection<Stat> statistic4 = new Collection<Stat>();
            var maxpro4 = from maxP in _context.Products
                          join noy in sumpro2 on maxP.ProductID equals noy.Key
                          select new
                          {
                              Category = maxP.Category,
                              Amount = noy.Amount
                          };
            //add categories
            var res4 = from maxC in maxpro4
                       group maxC by maxC.Category into fin
                       select new { Key = fin.Key, Amount = fin.Sum(fin => fin.Amount) };

            foreach (var v in res4)
            {
                statistic4.Add(new Stat(v.Key.ToString(), v.Amount));
            }
            ViewBag.stat4 = statistic4;

            return View();
        }
    }
}

public class Stat
{
    public string Key;
    public float Values;
    public Stat(string key, float values)
    {
        Key = key;
        Values = values;
    }
}
