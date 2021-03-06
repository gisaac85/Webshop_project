using Core.Dtos;
using Core.Entities.BasketModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Webshop.Shared;

namespace Webshop.Controllers
{
    public class BasketController : Controller
    {
        private static int countBasket = 0;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketController(IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Tuple<object, object, object>> PublicMethods()
        {
            var service = new SharedSpace(_httpContextAccessor);
            TempData["types"] = await service.FetchProductTypes();
            TempData["brands"] = await service.FetchProducBrands();
            var basketProducts = await service.FetchBasket();
            TempData["basketItems"] = basketProducts.Items.Count;
            return Tuple.Create(TempData["types"], TempData["brands"], TempData["basketItems"]);
        }


        public async Task<IActionResult> Index()
        {
            var service = new SharedSpace(_httpContextAccessor);
            var result = await service.FetchBasket();
            await PublicMethods();
            return View(result);
        }



        public async Task<IActionResult> AddToBasket(ProductToReturnDto product)
        {         
            var service = new SharedSpace(_httpContextAccessor);
            await service.AddProductToBasket(product);          
            return RedirectToAction("Index", "Products");
        }

        public IActionResult RemoveItem(int id)
        {
            return View();
        }
    }
}
