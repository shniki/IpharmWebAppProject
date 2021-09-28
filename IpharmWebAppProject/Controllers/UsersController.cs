using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IpharmWebAppProject.Data;
using IpharmWebAppProject.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace IpharmWebAppProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly IpharmContext _context;

        public UsersController(IpharmContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.
                Where(p => p.Active == true).ToListAsync());
        }

        //GET: Users with search
        [HttpGet]
        public IActionResult SearchEmailOrName(string query)
        {

            if (query == null || query == "")
            {
                var p = _context.Users.Where(p => p.Active == true)
                    .ToList();

                return PartialView("_UsersListView", p);
            }
            var products = _context.Users.Where(p => p.Active == true).
                Where(c => (c.FirstName.Contains(query) ||
                c.LastName.Contains(query) ||
                c.Email.Contains(query)))
                .ToList();

            return PartialView("_UsersListView", products);
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/PersonalArea
        public async Task<IActionResult> PersonalArea()
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == HttpContext.User.Claims.ElementAt(1).Value);

            if (!user.Active)
                return RedirectToAction("Logout");

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,FirstName,LastName,Birthday,Mobile,Password,PostalCode,Country,City,Adress")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (q == null)
                {
                    //orders
                    user.Orders = new List<Order>();
                    //cart
                    Order cart = new Order() {
                        Email = user.Email,
                        Status = Status.Cart,
                        Price = 0,
                        Products = new List<ProductInOrder>()};
                    user.Orders.Add(cart);
                    //reviews
                    user.Reviews = new List<Review>();
                    //wishlist
                    WishList wishlist = new WishList(){ Email = user.Email, Counter = 0 };
                    wishlist.Products = new List<ProductInWishList>();

                    _context.Add(user);
                    _context.Add(wishlist);
                    _context.Add(cart);

                    await _context.SaveChangesAsync();


                    var u= _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password==user.Password);
                    Signin(u);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Unable to comply;cannot register this user";
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] User user, string returnUrl)
        {
           
                var q = from u in _context.Users
                        where u.Email == user.Email && u.Password == user.Password && user.Active
                        select u;
                if (q.Count() > 0)
                {
                    // HttpContext.Session.SetString("Email", q.First().Email);

                    Signin(q.First());
                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect";
               
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            List<User> list = _context.Users.ToList();
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list.ElementAt(i).Email == account.Email)
                {
                    index = i;
                    continue;
                }
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.FirstName + " " + account.LastName), //0
                    new Claim(ClaimTypes.Email, account.Email), //1
                    new Claim(ClaimTypes.Name, account.FirstName), //2
                    new Claim(ClaimTypes.Name, account.LastName), //3
                    new Claim(ClaimTypes.DateOfBirth, account.Birthday.ToString()), //4
                    new Claim(ClaimTypes.MobilePhone, account.Mobile), //5
                    new Claim(ClaimTypes.PostalCode, account.PostalCode.ToString()), //6
                    new Claim(ClaimTypes.Country, account.Country), //7
                    new Claim(ClaimTypes.StateOrProvince, account.City), //8
                    new Claim(ClaimTypes.StreetAddress, account.Adress), //9
                    new Claim(ClaimTypes.Role, account.Type.ToString()), //10
                    new Claim(ClaimTypes.Rsa, index.ToString()) //11

                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit()
        {
            if (HttpContext.User == null || HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0) //not logged in
                return RedirectToAction("Login", "Users");

            var user = await _context.Users.FindAsync(HttpContext.User.Claims.ElementAt(1).Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,FirstName,LastName,Birthday,Mobile,Password,PostalCode,Country,City,Adress,Type,Active")] User user)
        {
            if (id != user.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PersonalArea");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            return RedirectToAction("NotFoundPage", "Home");

        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }
    }
}
