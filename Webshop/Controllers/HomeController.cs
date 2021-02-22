using Core.Entities.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;     
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product model)
        {
            try
            {
                // get filename
                var posted = Request.Form.Files["PictureUrl"];
                var imageName = Path.GetFileName(posted.FileName);

                // rewrite on model.pictururl
                model.PictureUrl = imageName;

                // save this image in wwwroot/images with the same name
                // update model.pictureurl with the Path of saved image


                Product productList = new Product();
                using (var httpClient = new HttpClient())
                {
                    var myContent = JsonConvert.SerializeObject(model);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:5001/api/products/createproduct", byteContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productList = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
