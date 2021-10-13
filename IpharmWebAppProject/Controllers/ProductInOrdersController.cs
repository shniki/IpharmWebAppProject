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
    public class ProductInOrdersController : Controller
    {
        private readonly IpharmContext _context;

        public ProductInOrdersController(IpharmContext context)
        {
            _context = context;
        }

        // GET: ProductInOrders
        public async Task<IActionResult> Index()
        {
            var ipharmContext = _context.ProductInOrders.Include(p => p.Order).Include(p => p.Product);
            return View(await ipharmContext.ToListAsync());
        }

        // GET: ProductInOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var productInOrder = await _context.ProductInOrders
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductInOrderId == id);
            if (productInOrder == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(productInOrder);
        }

        // GET: ProductInOrders/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: ProductInOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductInOrderId,Amount,OrderId,ProductId")] ProductInOrder productInOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", productInOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productInOrder.ProductId);
            return View(productInOrder);
        }

        // GET: ProductInOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var productInOrder = await _context.ProductInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", productInOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productInOrder.ProductId);
            return View(productInOrder);
        }

        // POST: ProductInOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductInOrderId,Amount,OrderId,ProductId")] ProductInOrder productInOrder)
        {
            if (id != productInOrder.ProductInOrderId)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInOrderExists(productInOrder.ProductInOrderId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", productInOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productInOrder.ProductId);
            return View(productInOrder);
        }

        // GET: ProductInOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var productInOrder = await _context.ProductInOrders
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductInOrderId == id);
            if (productInOrder == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(productInOrder);
        }

        // POST: ProductInOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInOrder = await _context.ProductInOrders.FindAsync(id);
            _context.ProductInOrders.Remove(productInOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInOrderExists(int id)
        {
            return _context.ProductInOrders.Any(e => e.ProductInOrderId == id);
        }
    }
}
