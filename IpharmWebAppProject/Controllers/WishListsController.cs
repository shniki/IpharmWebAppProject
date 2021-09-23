﻿using System;
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
        public async Task<IActionResult> Index(int? productid, bool addition)
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Manager") //logged in as manager
                return NotFound();

            //find user's cart in orders
            var mywishlist = await _context.WishLists.Include(wl => wl.Products)
                .FirstOrDefaultAsync(m => (m.Email == HttpContext.User.Claims.ElementAt(1).Value));

            if (mywishlist.Products == null)
            {
                mywishlist.Products = new List<ProductInWishList>();
            }

            if (productid != null) //there's a product
            {
                ProductInWishList productexists = (from p in mywishlist.Products where p.ProductID == productid select p).First();
                Product product = _context.Products.Find(productid);

                if (addition) //add product
                {
                    if (productexists == null) //not in cart
                    {
                        mywishlist.Products.Add(new ProductInWishList()
                        {
                            ProductID = productid.Value,
                            Product = product,
                            WishListID = mywishlist.Email,
                            WishList = mywishlist
                        });
                    }
                }
                else //remove product
                {
                    if (productexists != null) //in cart
                    {
                        mywishlist.Products.Remove(productexists);
                        _context.ProductInWishLists.Remove(productexists);
                    }
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

            _context.Update(mywishlist.Products);
            _context.SaveChanges();

            list = mywishlist.Products;

            //view wishlist only, without adding/removing products
            return View(list);
        }

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishLists
                .FirstOrDefaultAsync(m => m.Email == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // GET: WishLists/Create
        public IActionResult Create()
        {
            return View();
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
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
            return View(wishList);
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
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishLists
                .FirstOrDefaultAsync(m => m.Email == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
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
