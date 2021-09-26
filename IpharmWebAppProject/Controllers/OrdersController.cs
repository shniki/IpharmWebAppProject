using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IpharmWebAppProject.Data;
using IpharmWebAppProject.Models;

namespace IpharmWebAppProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IpharmContext _context;

        public OrdersController(IpharmContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        //GET: Users with search
        [HttpGet]
        public IActionResult SearchId(string query, string status, string price, string date)
        {
            var orders = _context.Orders.Where(p => p.Status != Status.Cart);
            var orders2 = _context.Orders.Where(p => p.Status != Status.Cart);
            if (query != null && query != "")
            {
                orders = orders2.Where(p => (p.OrderId.ToString().Contains(query)));
                orders2 = orders;
            }
            if (status != null && status != "")
            {
                orders = orders2.Where(p => (p.Status.ToString().Equals(status)));
                orders2 = orders;
            }
            if (price != null && price != "1")
            {
                switch (price)
                {
                    case "2":
                        orders = orders2.Where(p => (p.Price >= 0 && p.Price <= 25));
                        break;
                    case "3":
                        orders = orders2.Where(p => (p.Price >= 25 && p.Price <= 50));
                        break;
                    case "4":
                        orders = orders2.Where(p => (p.Price >= 50 && p.Price <= 100));
                        break;
                    case "5":
                        orders = orders2.Where(p => (p.Price >= 100));
                        break;
                }
                orders2 = orders;
            }
            if (price != null && price != "1")
            {
                switch (price)
                {
                    case "Week":
                        orders = orders2.Where(p => (p.OrderDate.AddDays(7).CompareTo(DateTime.Today)==1));
                        break;
                    case "Month":
                        orders = orders2.Where(p => (p.OrderDate.AddMonths(1).CompareTo(DateTime.Today) == 1));
                        break;
                    case "Year":
                        orders = orders2.Where(p => (p.OrderDate.AddYears(1).CompareTo(DateTime.Today) == 1));
                        break;
                }
                orders2 = orders;
            }

            var ret = orders2.ToList();
            return PartialView("_OrdersListView", ret);
        }

        // GET: Orders/Checkout
        public async Task<IActionResult> Checkout()
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Manager") //logged in as manager
                return NotFound();

            var myOrder = await _context.Orders.Where(o => o.Email == HttpContext.User.Claims.ElementAt(1).Value && o.Status == Status.Cart).FirstOrDefaultAsync();

            myOrder.Status = Status.Paid;
            myOrder.OrderDate = DateTime.Now;

            _context.Orders.Update(myOrder);
            _context.Orders.Add(new Order { Email = HttpContext.User.Claims.ElementAt(1).Value, Price = 0, Status = Status.Cart, Products = new List<ProductInOrder>() });
            _context.SaveChanges();

            return View(myOrder);
        }

        // Post: Orders/Cart
        public async Task<IActionResult> Cart(int? productid, bool addition, bool wishlist)
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value=="Manager") //logged in as manager
                return NotFound();

            //find user's cart in orders
            var mycart = await _context.Orders.Include(o => o.Products)
                .FirstOrDefaultAsync(m => (m.Email == HttpContext.User.Claims.ElementAt(1).Value && m.Status==Status.Cart));
            
            if (mycart.Products == null)
            {
                mycart.Products = new List<ProductInOrder>();
            }

            if (productid!=null) //there's a product
            {
                ProductInOrder productexists = (from p in mycart.Products where p.ProductId == productid select p).First();
                Product product = _context.Products.Find(productid);

                if (addition) //add product
                {
                    if (productexists != null) //in cart
                    {
                        productexists.Amount += 1;
                    }
                    else //not in cart
                    {
                        mycart.Products.Add(new ProductInOrder() {  ProductId=productid.Value, Product= product,
                                                                    OrderId=mycart.OrderId, Order=mycart,
                                                                    Amount = 1});
                    }
                    mycart.Price += product.Price;
                    product.Stock -= 1;
                    
                    if(wishlist) //need to remove from wishlist
                    {
                        var mywishlist = await _context.WishLists.Include(o => o.Products).FirstOrDefaultAsync(m => (m.Email == HttpContext.User.Claims.ElementAt(1).Value));
                        ProductInWishList productwl = (from p in mywishlist.Products where p.ProductId == productid select p).First();
                        mywishlist.Products.Remove(productwl);
                        _context.ProductInWishLists.Remove(productwl);
                        _context.WishLists.Update(mywishlist);
                        _context.SaveChanges();
                    }
                }
                else //remove product
                {
                    if (productexists != null) //in cart
                    {
                        mycart.Price-=(product.Price*productexists.Amount);
                        mycart.Products.Remove(productexists);
                        _context.ProductInOrders.Remove(productexists);
                        product.Stock += productexists.Amount;
                    }
                }
                _context.Orders.Update(mycart);
                _context.Products.Update(product);
                _context.SaveChanges();
            }

            var list = mycart.Products;

            foreach (ProductInOrder p in list)
            {
                if (!p.Product.Active)
                {
                    mycart.Price -= (p.Product.Price * p.Amount);
                    mycart.Products.Remove(p);
                }
            }

            _context.Update(mycart.Products);
            _context.Orders.Update(mycart);
            _context.SaveChanges();

            list = mycart.Products;

            //view cart only, without adding/removing products
            return View(list);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,Email,Status,Price,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Email,Status,Price,OrderDate")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
