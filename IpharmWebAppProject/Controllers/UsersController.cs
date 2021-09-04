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
            return View(await _context.Users.ToListAsync());
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
                    _context.Add(user);
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
        public async Task<IActionResult> Login([Bind("Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.Users
                        where u.Email == user.Email && u.Password == user.Password
                        select u;
                if (q.Count() > 0)
                {
                    // HttpContext.Session.SetString("Email", q.First().Email);

                    Signin(q.First());
                    
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
          /*  List<User> list = _context.Users.ToList();
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list.ElementAt(i).Email == account.Email)
                {
                    index = i;
                    continue;
                }
            }*/
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.FirstName + " " + account.LastName),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.MobilePhone, account.Mobile),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                    new Claim(ClaimTypes.StreetAddress, account.Adress),
                    new Claim(ClaimTypes.StateOrProvince, account.City),
                    new Claim(ClaimTypes.PostalCode, account.PostalCode.ToString()),
                    new Claim(ClaimTypes.Country, account.Country),
                    //new Claim(ClaimTypes.Rsa, index.ToString()),
                    new Claim(ClaimTypes.Name, account.FirstName),
                    new Claim(ClaimTypes.Name, account.LastName),
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
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
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
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
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
