using Core.Entities.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Webshop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: ProductsController
        public async Task<IActionResult> Index()
        {
            List<Product> productList = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/products/getallproducts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }           
            return View(productList);
        }

        // GET: ProductsController/Details/5       
        public async Task<IActionResult> SearchProduct(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index", "Products");
            }
            List<Product> productList = new List<Product>();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getproductbyname/{name}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();                    
                        productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);                        
                    }
                }
                if (productList.Any(i => i == null))
                {
                    TempData["Error"] = "Product Not Found";
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    TempData["message"] = $"{productList.Count()} products found";
                }
            }
            return View("Index", productList);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
