using Core.Dtos;
using Core.Entities.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Webshop.SharedMethods;

namespace Webshop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: ProductsController
        public async Task<IActionResult> Index()
        {
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/products/getallproducts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<ProductToReturnDto>>(apiResponse);
                }
            }
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();           
            return View(productList);
        }     

        // GET
        public async Task<IActionResult> GetProductByProductId(int id)
        {
            ProductToReturnDto productList = new ProductToReturnDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getproduct/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<ProductToReturnDto>(apiResponse);
                }
            }
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return View(productList);
        }

        // GET: ProductsController/SearchProducts/productName      
        public async Task<IActionResult> SearchProduct(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index", "Products");
            }
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getproductbyname/{name}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();                    
                        productList = JsonConvert.DeserializeObject<List<ProductToReturnDto>>(apiResponse);                        
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
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return View("Index", productList);
        }

        // GET: ProductsController/FetchProductsbyBrandName/brandId
        public async Task<IActionResult> FetchProductsByBrandName(int brandId)
        {
            if (brandId==0 ||brandId<0)
            {
                return RedirectToAction("Index", "Products");
            }
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getproductsbybrand/{brandId}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productList = JsonConvert.DeserializeObject<List<ProductToReturnDto>>(apiResponse);
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
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return View("Index", productList);

        }

        // GET: ProductsController/FetchProductsbyType/typeId
        public async Task<IActionResult> FetchProductsByType(int typeId)
        {
            if (typeId == 0 || typeId < 0)
            {
                return RedirectToAction("Index", "Products");
            }
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getproductsbytype/{typeId}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productList = JsonConvert.DeserializeObject<List<ProductToReturnDto>>(apiResponse);
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
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return View("Index", productList);
        }

        // GET: ProductsController/SortProductByPrice      
        public async Task<IActionResult> FilterProduct(int filter)
        {
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getsortproductbyprice?filter={filter}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<ProductToReturnDto>>(apiResponse);
                }
            }
            var sharedMethod = new SharedSpace();
            TempData["types"] = await sharedMethod.FetchProductTypes();
            TempData["brands"] = await sharedMethod.FetchProducBrands();
            return View("Index", productList);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ProductToReturnDto product = new ProductToReturnDto();

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:5001/api/products/getproduct/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<ProductToReturnDto>(apiResponse);
                    }
                }
            }

            return PartialView("_Details", product);
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
