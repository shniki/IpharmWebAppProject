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
    public class WishListsController : Controller
    {
        private readonly IpharmContext _context;

        public WishListsController(IpharmContext context)
        {
            _context = context;
        }

        // GET: WishLists
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("NotFoundPage", "Home");
        }

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(int? productid, bool addition)
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Manager") //logged in as manager
                return NotFound();

            //find user's cart in orders
            var mywishlist = _context.WishLists.Include(wl => wl.Products).ThenInclude(p=>p.Product)
                .FirstOrDefault(m => (m.Email == HttpContext.User.Claims.ElementAt(1).Value));

            if (mywishlist.Products == null)
            {
                mywishlist.Products = new List<ProductInWishList>();
            }

            if (productid != null) //there's a product
            {
                ProductInWishList productexists = mywishlist.Products.Where(p => p.ProductId == productid).FirstOrDefault();
                Product product = _context.Products.Find(productid);

                if (addition) //add product
                {
                    if (productexists == null) //not in cart
                    {
                        mywishlist.Products.Add(new ProductInWishList()
                        {
                            ProductId = productid.Value,
                            Product = product,
                            WishListId = mywishlist.Email,
                            WishList = mywishlist
                        });
                        mywishlist.Counter += 1;
                    }
                }
                else //remove product
                {
                    if (productexists != null) //in cart
                    {
                        mywishlist.Products.Remove(productexists);
                        _context.ProductInWishLists.Remove(productexists);
                    }
                    mywishlist.Counter -= 1;
                }
                _context.WishLists.Update(mywishlist);
                _context.SaveChanges();
            }

            var list = mywishlist.Products;

            foreach (ProductInWishList p in list)
            {
                if(!p.Product.Active)
                    mywishlist.Products.Remove(p);
            }

            //_context.Update(mywishlist.Products);
            _context.SaveChanges();

            //view wishlist only, without adding/removing products
            return View(mywishlist);
        }

        // GET: WishLists/Create
        public IActionResult Create()
        {
            return RedirectToAction("NotFoundPage", "Home");
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Counter")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishList);
        }

        // GET: WishLists/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            return RedirectToAction("NotFoundPage", "Home");

        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Counter")] WishList wishList)
        {
            if (id != wishList.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.Email))
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
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            return RedirectToAction("NotFoundPage", "Home");

        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var wishList = await _context.WishLists.FindAsync(id);
            _context.WishLists.Remove(wishList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListExists(string id)
        {
            return _context.WishLists.Any(e => e.Email == id);
        }
    }
}
