using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WorkWithBankAPI.Models;

namespace WorkWithBankAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!memoryCache.TryGetValue("key currency", out CurrencyConverter model))
            {
                throw new Exception("We have error about gtting data");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ConvertSum(string sum)
        {
            ViewBag.Sum = sum;
            if (!memoryCache.TryGetValue("key currency", out CurrencyConverter model))
            {
                throw new Exception("We have error about gtting data");
            }
            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
