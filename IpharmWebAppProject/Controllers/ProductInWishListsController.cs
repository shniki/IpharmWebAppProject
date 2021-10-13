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
    public class ProductInWishListsController : Controller
    {
        private readonly IpharmContext _context;

        public ProductInWishListsController(IpharmContext context)
        {
            _context = context;
        }

        // GET: ProductInWishLists
        public async Task<IActionResult> Index()
        {
            var ipharmContext = _context.ProductInWishLists.Include(p => p.Product);
            return View(await ipharmContext.ToListAsync());
        }

        // GET: ProductInWishLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var productInWishList = await _context.ProductInWishLists
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductInWishListId == id);
            if (productInWishList == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(productInWishList);
        }

        // GET: ProductInWishLists/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: ProductInWishLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductInWishListId,WishListId,ProductId")] ProductInWishList productInWishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInWishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productInWishList.ProductId);
            return View(productInWishList);
        }

        // GET: ProductInWishLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var productInWishList = await _context.ProductInWishLists.FindAsync(id);
            if (productInWishList == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productInWishList.ProductId);
            return View(productInWishList);
        }

        // POST: ProductInWishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductInWishListId,WishListId,ProductId")] ProductInWishList productInWishList)
        {
            if (id != productInWishList.ProductInWishListId)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInWishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInWishListExists(productInWishList.ProductInWishListId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productInWishList.ProductId);
            return View(productInWishList);
        }

        // GET: ProductInWishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var productInWishList = await _context.ProductInWishLists
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductInWishListId == id);
            if (productInWishList == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(productInWishList);
        }

        // POST: ProductInWishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInWishList = await _context.ProductInWishLists.FindAsync(id);
            _context.ProductInWishLists.Remove(productInWishList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInWishListExists(int id)
        {
            return _context.ProductInWishLists.Any(e => e.ProductInWishListId == id);
        }
    }
}
