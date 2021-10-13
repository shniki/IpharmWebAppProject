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
using System.IO;
using System.Web;

namespace IpharmWebAppProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IpharmContext _context;

        public ProductsController(IpharmContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Where(p => p.Active == true).ToListAsync());
        }

        //GET: Products with search
        [HttpGet]
        public IActionResult SearchIdOrName(string query)
        {
            if (query == null || query == "")
            {
                var p = _context.Products.Where(p => p.Active == true)
                    .ToList();

                return PartialView("_ProductsListView", p);
            }
            var products =  _context.Products.Where(p => p.Active == true).
                Where(c => c.Name.Contains(query)||c.ProductId.ToString().Contains(query))
                .ToList();

            return PartialView("_ProductsListView",products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var product = await _context.Products.Include(r => r.Reviews)
                .FirstOrDefaultAsync(m => m.ProductId == id && m.Active);
            if (product == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
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
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Amount,Gender,Category,Type,Brand,Description,Rate,PicUrl1,PicUrl2,PicUrl3,Stock,Active")] Product product)
        {
            //if (ModelState.IsValid)
            //{
                product.Reviews = new List<Review>();
                product.InWishList = new List<ProductInWishList>();
                product.InOrders = new List<ProductInOrder>();
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            //ViewBag.brands = await _context.Products.GroupBy(p => p.Brand).Select(g => g.Key).ToListAsync();
            ViewBag.types = await _context.Products.GroupBy(p => p.Type).Select(g => g.Key).ToListAsync();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Amount,Gender,Category,Type,Brand,Description,Rate,PicUrl1,PicUrl2,PicUrl3,Stock,Active")] Product product)
        {
            if (id != product.ProductId)
            {
                return RedirectToAction("NotFoundPage", "Home");
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
                    if (!ProductExists(product.ProductId))
                    {
                        return RedirectToAction("NotFoundPage", "Home");
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
                return RedirectToAction("NotFoundPage", "Home");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(product);
        }
        public async Task<IActionResult> Search(string query, string sort="0", string gender="0", string category="0", string price="0")
        {
            ViewBag.query = HttpUtility.UrlEncode(query);
            ViewBag.sort = sort;
            ViewBag.gender = gender;
            ViewBag.category = category;
            ViewBag.price = price;

            if (query == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = _context.Products.Where(c => c.Name.Contains(query)&&c.Active);
            var temp = products;
            if (sort != null && sort != "0" && sort != "1")
            {
                switch (sort)
                {
                    case "2": products = from p in temp orderby p.Price descending select p; break;
                    case "3": products = from p in temp orderby p.Price select p;  break;
                    case "4": products = from p in temp orderby p.Rate descending select p; break;
                }
                temp = products;
            }
            if (price != null && price != "0")
            {
                switch (price)
                {
                    case "1":
                        products = temp.Where(p => (p.Price >= 0 && p.Price <= 25));
                        break;
                    case "2":
                        products = temp.Where(p => (p.Price >= 25 && p.Price <= 50));
                        break;
                    case "3":
                        products = temp.Where(p => (p.Price >= 50 && p.Price <= 100));
                        break;
                    case "4":
                        products = temp.Where(p => (p.Price >= 100));
                        break;
                }
                temp = products;
            }
            if (gender != null && gender != "0")
            {
                switch (gender)
                {
                    case "1":
                        products = temp.Where(p =>p.Gender==Genders.Women);
                        break;
                    case "2":
                        products = temp.Where(p => p.Gender == Genders.Men);
                        break;
                    case "3":
                        products = temp.Where( p => p.Gender == Genders.Unisex);
                        break;

                }
                temp = products;
            }
            if (category != null && category != "0")
            {
                switch (category)
                {
                    case "1":
                        products = temp.Where(p => p.Category == Categories.Skincare);
                        break;
                    case "2":
                        products = temp.Where(p => p.Category == Categories.Haircare);
                        break;
                    case "3":
                        products = temp.Where(p => p.Category == Categories.Makeup);
                        break;

                }
                temp = products;
            }
            ViewBag.searchSort = sort;
            ViewBag.searchGender = gender;
            ViewBag.searchPrice = price;
            ViewBag.searchCategory = category;
            var ret = temp.ToList();
            return View(ret);
        }

        public async Task<IActionResult> Category(string category, string sort = "0", string gender = "0", string price = "0")
        {
            ViewBag.sort = sort;
            ViewBag.gender = gender;
            ViewBag.category = category;
            ViewBag.price = price;

            if (category == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            Categories categoryval = Categories.Skincare;
            switch (category)
            {
                case "Skincare":
                    categoryval = Categories.Skincare;
                    break;
                case "Haircare":
                    categoryval = Categories.Haircare;
                    break;
                case "Makeup":
                    categoryval = Categories.Makeup;
                    break;
            }

            var products = _context.Products.Where(c => c.Category==categoryval &&c.Active);
            var temp = products;
            if (sort != null && sort != "0" && sort != "1")
            {
                switch (sort)
                {
                    case "2": products = from p in temp orderby p.Price descending select p; break;
                    case "3": products = from p in temp orderby p.Price select p; break;
                    case "4": products = from p in temp orderby p.Rate descending select p; break;
                }
                temp = products;
            }
            if (price != null && price != "0")
            {
                switch (price)
                {
                    case "1":
                        products = temp.Where(p => (p.Price >= 0 && p.Price <= 25));
                        break;
                    case "2":
                        products = temp.Where(p => (p.Price >= 25 && p.Price <= 50));
                        break;
                    case "3":
                        products = temp.Where(p => (p.Price >= 50 && p.Price <= 100));
                        break;
                    case "4":
                        products = temp.Where(p => (p.Price >= 100));
                        break;
                }
                temp = products;
            }
            if (gender != null && gender != "0")
            {
                switch (gender)
                {
                    case "1":
                        products = temp.Where(p => p.Gender == Genders.Women);
                        break;
                    case "2":
                        products = temp.Where(p => p.Gender == Genders.Men);
                        break;
                    case "3":
                        products = temp.Where(p => p.Gender == Genders.Unisex);
                        break;

                }
                temp = products;
            }

            ViewBag.searchSort = sort;
            ViewBag.searchGender = gender;
            ViewBag.searchPrice = price;
            ViewBag.searchCategory = category;
            var ret = temp.ToList();
            return View(ret);
        }

        public async Task<IActionResult> Brand(string brand, string sort = "0", string gender = "0", string category = "0", string price = "0")
        {
            ViewBag.brand = brand;
            ViewBag.sort = sort;
            ViewBag.gender = gender;
            ViewBag.category = category;
            ViewBag.price = price;

            if (brand == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            var products = _context.Products.Where(c => c.Brand.Equals(brand) && c.Active);
            var temp = products;
            if (sort != null && sort != "0" && sort != "1")
            {
                switch (sort)
                {
                    case "2": products = from p in temp orderby p.Price descending select p; break;
                    case "3": products = from p in temp orderby p.Price select p; break;
                    case "4": products = from p in temp orderby p.Rate descending select p; break;
                }
                temp = products;
            }
            if (price != null && price != "0")
            {
                switch (price)
                {
                    case "1":
                        products = temp.Where(p => (p.Price >= 0 && p.Price <= 25));
                        break;
                    case "2":
                        products = temp.Where(p => (p.Price >= 25 && p.Price <= 50));
                        break;
                    case "3":
                        products = temp.Where(p => (p.Price >= 50 && p.Price <= 100));
                        break;
                    case "4":
                        products = temp.Where(p => (p.Price >= 100));
                        break;
                }
                temp = products;
            }
            if (gender != null && gender != "0")
            {
                switch (gender)
                {
                    case "1":
                        products = temp.Where(p => p.Gender == Genders.Women);
                        break;
                    case "2":
                        products = temp.Where(p => p.Gender == Genders.Men);
                        break;
                    case "3":
                        products = temp.Where(p => p.Gender == Genders.Unisex);
                        break;

                }
                temp = products;
            }
            if (category != null && category != "0")
            {
                switch (category)
                {
                    case "1":
                        products = temp.Where(p => p.Category == Categories.Skincare);
                        break;
                    case "2":
                        products = temp.Where(p => p.Category == Categories.Haircare);
                        break;
                    case "3":
                        products = temp.Where(p => p.Category == Categories.Makeup);
                        break;

                }
                temp = products;
            }
            ViewBag.searchSort = sort;
            ViewBag.searchGender = gender;
            ViewBag.searchPrice = price;
            ViewBag.searchCategory = category;
            var ret = temp.ToList();
            return View(ret);
        }

        public async Task<IActionResult> New(string sort = "0", string gender = "0", string category = "0", string price = "0")
        {
            ViewBag.sort = sort;
            ViewBag.gender = gender;
            ViewBag.category = category;
            ViewBag.price = price;

            var products = (from p in _context.Products where p.Active orderby p.ProductId descending select p).Take(9);
            var temp = products;
            if (sort != null && sort != "0" && sort != "1")
            {
                switch (sort)
                {
                    case "2": products = from p in temp orderby p.Price descending select p; break;
                    case "3": products = from p in temp orderby p.Price select p; break;
                    case "4": products = from p in temp orderby p.Rate descending select p; break;
                }
                temp = products;
            }
            if (price != null && price != "0")
            {
                switch (price)
                {
                    case "1":
                        products = temp.Where(p => (p.Price >= 0 && p.Price <= 25));
                        break;
                    case "2":
                        products = temp.Where(p => (p.Price >= 25 && p.Price <= 50));
                        break;
                    case "3":
                        products = temp.Where(p => (p.Price >= 50 && p.Price <= 100));
                        break;
                    case "4":
                        products = temp.Where(p => (p.Price >= 100));
                        break;
                }
                temp = products;
            }
            if (gender != null && gender != "0")
            {
                switch (gender)
                {
                    case "1":
                        products = temp.Where(p => p.Gender == Genders.Women);
                        break;
                    case "2":
                        products = temp.Where(p => p.Gender == Genders.Men);
                        break;
                    case "3":
                        products = temp.Where(p => p.Gender == Genders.Unisex);
                        break;

                }
                temp = products;
            }
            if (category != null && category != "0")
            {
                switch (category)
                {
                    case "1":
                        products = temp.Where(p => p.Category == Categories.Skincare);
                        break;
                    case "2":
                        products = temp.Where(p => p.Category == Categories.Haircare);
                        break;
                    case "3":
                        products = temp.Where(p => p.Category == Categories.Makeup);
                        break;

                }
                temp = products;
            }
            ViewBag.searchSort = sort;
            ViewBag.searchGender = gender;
            ViewBag.searchPrice = price;
            ViewBag.searchCategory = category;
            var ret = temp.ToList();
            return View(ret);
        }

        public async Task<IActionResult> FriendsCollection()
        {
            var products = _context.Products.Where(c => c.Name.Contains("friends") && c.Active).ToList();
            return View(products);
        }

        public async Task<IActionResult> AllBrands()
        {
            var brands = (from prd in _context.Products
                          where prd.Active
                          group prd by prd.Brand into prdb
                          select new { Brand = prdb.Key }).ToList();
            List<String> ret = new List<string>();
            foreach (var item in brands)
            {
                ret.Add(item.Brand);
            }
            return View(ret);
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
            return _context.Products.Any(e => e.ProductId == id);
        }

        [HttpGet]
        public ActionResult Statistics()
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Customer") //logged in as customer
                return RedirectToAction("Index", "Home");

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
                              select new { key = o.OrderId }; //orders from the past year
            var sumpro2 = from proinord in _context.ProductInOrders
                          join pro in yearorders2 on proinord.OrderId equals pro.key
                          group proinord by proinord.ProductId into products2
                          select new { Key = products2.Key, Amount = products2.Sum(products2 => products2.Amount) };
            //amount bought of each product (in past year)
            var maxpro2 = (from maxP in _context.Products
                           join noy in sumpro2 on maxP.ProductId equals noy.Key
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
                          join noy in sumpro2 on maxP.ProductId equals noy.Key
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
