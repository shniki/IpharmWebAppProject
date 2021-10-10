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
    public class ReviewsController : Controller
    {
        private readonly IpharmContext _context;

        public ReviewsController(IpharmContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0
                && HttpContext.User.Claims.ElementAt(10).Value == "Manager") //logged in as manager
                return NotFound();

            var rev = _context.Reviews.Where(p => p.UserEmail == HttpContext.User.Claims.ElementAt(1).Value).ToList();
            return View(rev);
        }

        //GET: Reviews with search
        [HttpGet]
        public IActionResult SearchTitle(string query)
        {

            if (query == null || query == "")
            {
                var rev = _context.Reviews.Where(p => p.UserEmail == HttpContext.User.Claims.ElementAt(1).Value).ToList();

                return PartialView("_ReviewsListView", rev);
            }
            var rev2 = _context.Reviews.Where(p => p.UserEmail == HttpContext.User.Claims.ElementAt(1).Value
            &&p.Title.Contains(query)).ToList();


            return PartialView("_ReviewsListView", rev2);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,ProductId,UserEmail,Title,Description,Rate")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                var product=_context.Products.Include(p=>p.Reviews).Where(p => p.ProductId == review.ProductId).FirstOrDefault();
                if (product.Reviews.Count()>1)
                {
                    product.Rate *= (product.Reviews.Count()-1);
                    product.Rate += review.Rate;
                    product.Rate /= (product.Reviews.Count());
                }
                else
                product.Rate= review.Rate;
                //product.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Products", new { id = review.ProductId });
            }
            return View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Submitted(int id, string title, string desc, int rate)
        {
            if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0 && HttpContext.User.Claims.ElementAt(10).Value == "Customer")
            {
                Review review = new Review();
                review.ProductId = id;
                review.UserEmail = HttpContext.User.Claims.ElementAt(1).Value;
                review.Title = title;
                review.Description = desc;
                review.Rate = rate;

                _context.Add(review);
                var product = _context.Products.Include(p => p.Reviews).Where(p => p.ProductId == review.ProductId).FirstOrDefault();
                if (product.Reviews.Count() > 1)
                {
                    product.Rate *= (product.Reviews.Count() - 1);
                    product.Rate += review.Rate;
                    product.Rate /= (product.Reviews.Count());
                }
                else
                    product.Rate = review.Rate;
                //product.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Products", new { id = review.ProductId });
            }
            else if (HttpContext.User != null && HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0 && HttpContext.User.Claims.ElementAt(10).Value == "Customer")
                return NotFound();
            else
                return RedirectToAction("Login", "Users");
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,ProductId,UserEmail,Title,Description,Rate")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            var product = _context.Products.Include(p => p.Reviews).Where(p => p.ProductId == review.ProductId).FirstOrDefault();
            //product.Reviews.Remove(review);
            _context.Reviews.Remove(review);
            if (product.Reviews.Count() > 1)
            {
                product.Rate *= product.Reviews.Count();
                product.Rate -= review.Rate;
                product.Rate /= (product.Reviews.Count() - 1);
            }
            else
                product.Rate = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
