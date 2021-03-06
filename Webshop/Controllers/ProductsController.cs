using Core.Dtos;
using Core.Entities.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Webshop.Models.ViewModels;
using Webshop.Shared;
using Webshop.Shared.Interfaces;

namespace Webshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductMVCRepository _productMVCRepo;

        public ProductsController(IHttpContextAccessor httpContextAccessor,IProductMVCRepository productMVCRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _productMVCRepo = productMVCRepo;
        }

        public async Task<Tuple<object,object,object>> PublicMethods()
        {
            var service = new SharedSpace(_httpContextAccessor);
            TempData["types"] = await service.FetchProductTypes(); 
            TempData["brands"] = await service.FetchProducBrands();
            var basketProducts = await service.FetchBasket();
            TempData["basketItems"] = basketProducts.Items.Count;
            return Tuple.Create(TempData["types"], TempData["brands"], TempData["basketItems"]);
        }

        // GET: ProductsController
        public async Task<IActionResult> Index()
        {
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();            
            productList = await _productMVCRepo.GetAllAsync(ApiUri.ProductBaseUri + "getallproducts");            
            await PublicMethods();
            return View(productList);
        }

        // GET
        public async Task<IActionResult> GetProductByProductId(int id)
        {
            ProductToReturnDto product = new ProductToReturnDto();           
            product = await _productMVCRepo.GetAsync(ApiUri.ProductBaseUri + "getproduct/", id);
            await PublicMethods();
            return View(product);
        }

        // GET: ProductsController/SearchProducts/productName      
        public async Task<IActionResult> SearchProduct(string name)
        {
            //CheckAuth();
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index", "Products");
            }

            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();

            if (ModelState.IsValid)
            {               
                productList = await _productMVCRepo.GetListByNameAsync(ApiUri.ProductBaseUri + "getproductbyname/", name);

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
            await PublicMethods();
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
                productList = await _productMVCRepo.GetListByIdAsync(ApiUri.ProductBaseUri + "getproductsbybrand/", brandId);
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
            await PublicMethods();
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
                productList = await _productMVCRepo.GetListByIdAsync(ApiUri.ProductBaseUri + "getproductsbytype/", typeId);
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
            await PublicMethods();
            return View("Index", productList);
        }

        // GET: ProductsController/SortProductByPrice      
        public async Task<IActionResult> FilterProduct(int filter)
        {
            List<ProductToReturnDto> productList = new List<ProductToReturnDto>();           
            productList = await _productMVCRepo.GetListByIdAsync(ApiUri.ProductBaseUri + "getsortproductbyprice?filter=", filter);
            await PublicMethods();
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
