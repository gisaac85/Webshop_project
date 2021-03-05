using Core.Entities.BasketModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Shared;

namespace Webshop.Controllers
{
    public class BasketController : Controller
    {
        public async Task<Tuple<object, object>> PublicMethods()
        {
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return Tuple.Create(TempData["types"], TempData["brands"]);
        }
        public async Task<IActionResult> Index()
        {
            var basket = new CustomerBasket();




            await PublicMethods();
            return View();
        }
    }
}
