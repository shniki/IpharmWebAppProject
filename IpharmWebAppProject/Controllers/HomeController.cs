using IpharmWebAppProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IpharmWebAppProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _posts;

        private string getPosts()
        {
            var url = "https://api.twitter.com/1.1/search/tweets.json?q=%23superbowl&result_type=recent";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = "Bearer AAAAAAAAAAAAAAAAAAAAABWVUQEAAAAA3pjPn0RXe93LEdgfdW7f1JdHhbI%3DTtCR0sZtohOeFmmo20FuQeBaXWnp26tWTktL4ZA20ziM81e5VA";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _posts = getPosts();
        }

        public IActionResult Index()
        {
            return View();
        }
        //[Authorize]
        public IActionResult Privacy()
        {
            //if (HttpContext.Session.GetString("Email") == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
