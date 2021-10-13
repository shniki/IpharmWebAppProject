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
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Manager") //logged in as manager
            return View(await _context.Orders.Where(p=> p.Status != Status.Cart).ToListAsync());
            else
                return View(await _context.Orders.Where(p=>p.Email== HttpContext.User.Claims.ElementAt(1).Value && p.Status!=Status.Cart).ToListAsync());
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
                switch (status)
                {
                    case "Paid":
                        orders = orders2.Where(p => (p.Status==Status.Paid));
                        break;
                    case "Arrived":
                        orders = orders2.Where(p => (p.Status==Status.Arrived));
                        break;
                }
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
            if (date != null && date != "1")
            {
                switch (date)
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

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Customer") //logged in as customer
                        orders2.Where(p => p.Email == HttpContext.User.Claims.ElementAt(1).Value);

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
                  return RedirectToAction(nameof(Index), "Home");

            var myOrder = await _context.Orders.Where(o => o.Email == HttpContext.User.Claims.ElementAt(1).Value && o.Status == Status.Cart).FirstOrDefaultAsync();

            myOrder.Status = Status.Paid;
            myOrder.OrderDate = DateTime.Now;
            if (myOrder.Price <= 20)
                myOrder.Price = myOrder.Price + 5;

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
                return RedirectToAction(nameof(Index), "Home");

            //find user's cart in orders
            var mycart = _context.Orders.Include(o => o.Products).ThenInclude(p=>p.Product)
                .FirstOrDefault(m => (m.Email == HttpContext.User.Claims.ElementAt(1).Value && m.Status==Status.Cart));
            
            if (mycart.Products == null)
            {
                mycart.Products = new List<ProductInOrder>();
            }

            if (productid!=null) //there's a product
            {
                ProductInOrder productexists = mycart.Products.Where(p=>p.ProductId == productid).FirstOrDefault();
                Product product = _context.Products.Find(productid);

                if (addition) //add product
                {
                    if (productexists != null) //in cart
                    {
                        productexists.Amount += 1;
                    }
                    else //not in cart
                    {
                        mycart.Products.Add(new ProductInOrder()
                        {
                            ProductId = productid.Value,
                            Product = product,
                            OrderId = mycart.OrderId,
                            Order = mycart,
                            Amount = 1
                        });
                    }
                    mycart.Price += product.Price;
                    product.Stock -= 1;
                    
                    if(wishlist) //need to remove from wishlist
                    {
                        var mywishlist = _context.WishLists.Include(o => o.Products).FirstOrDefault(m => (m.Email == HttpContext.User.Claims.ElementAt(1).Value));
                        ProductInWishList productwl = (from p in mywishlist.Products where p.ProductId == productid select p).First();
                        _context.ProductInWishLists.Remove(productwl);
                        mywishlist.Counter -= 1;
                        _context.Update(mywishlist);
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
                _context.SaveChanges();
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

            //_context.Update(mycart.Products);
            _context.Orders.Update(mycart);
            _context.SaveChanges();

            //view cart only, without adding/removing products
            return View(mycart);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

                var order = await _context.Orders.Include(r=>r.Products).ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            //ViewBag.listProducts = order.Products;
            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && (HttpContext.User.Claims.ElementAt(10).Value == "Manager" || HttpContext.User.Claims.ElementAt(1).Value == order.Email)) //logged in as manager or as user made
                    return View(order);

            return RedirectToAction("NotFoundPage", "Home");
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return RedirectToAction("NotFoundPage", "Home");

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
            return RedirectToAction("NotFoundPage", "Home");
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

        //update status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Arrived(int? id)
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

            order.Status = Status.Arrived;

            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //return RedirectToAction("NotFoundPage", "Home");
            var review = await _context.Orders
             .FirstOrDefaultAsync(m => m.OrderId == id);
            if (review == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(review);
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
