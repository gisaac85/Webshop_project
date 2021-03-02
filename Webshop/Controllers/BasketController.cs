using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.SharedMethods;

namespace Webshop.Controllers
{
    public class BasketController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return View();
        }
    }
}
