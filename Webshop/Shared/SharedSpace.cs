using Core.Entities.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Core.Entities.BasketModels;
using Core.Dtos;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Webshop.Shared
{
    public class SharedSpace
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        CustomerBasket basket = new CustomerBasket();
        CustomerBasket output = new CustomerBasket();
        BasketItem basketItem = new BasketItem();
        private static string basketID,basketInSession = string.Empty;
        //List<BasketItem> x = new List<BasketItem>();

        public SharedSpace(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            basketID = _httpContextAccessor.HttpContext.Session.GetString("basketId");
            basketInSession = _httpContextAccessor.HttpContext.Session.GetString("output");
            //if (items != null)
            //{
            //    var obj = JsonConvert.DeserializeObject<List<BasketItem>>(items);
            //    x = obj;
            //}
        }

        public async Task<List<ProductType>> FetchProductTypes()
        {
            List<ProductType> productTypeList = new List<ProductType>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/products/gettypes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productTypeList = JsonConvert.DeserializeObject<List<ProductType>>(apiResponse);
                }
            }
            return productTypeList;
        }

        public async Task<List<ProductBrand>> FetchProducBrands()
        {
            List<ProductBrand> productBrandList = new List<ProductBrand>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/products/getbrands"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productBrandList = JsonConvert.DeserializeObject<List<ProductBrand>>(apiResponse);
                }
            }
            return productBrandList;
        }

        public async Task<bool> AddProductToBasket(ProductToReturnDto product)
        {
            if (basketID == null)
            {
                basketItem.Id = product.Id;
                basketItem.PictureUrl = product.PictureUrl;
                basketItem.Price = product.Price;
                basketItem.ProductName = product.Name;
                basketItem.Type = product.ProductType;
                basketItem.Brand = product.ProductBrand;
                basketItem.Quantity = 1;

                Guid id = Guid.NewGuid(); // create basketID for everysession, we use GUID number 
                basket.Id = id.ToString(); 
                basket.Items.Add(basketItem);

                using (var httpClient = new HttpClient())
                {
                    var myContent = JsonConvert.SerializeObject(basket);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:5001/api/basket/updatebasket", byteContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse);
                        _httpContextAccessor.HttpContext.Session.SetString("basketId", output.Id);
                        _httpContextAccessor.HttpContext.Session.SetString("output", JsonConvert.SerializeObject(output.Items));
                    }
                }
            }
            else
            {
                basketItem.Id = product.Id;
                basketItem.PictureUrl = product.PictureUrl;
                basketItem.Price = product.Price;
                basketItem.ProductName = product.Name;
                basketItem.Type = product.ProductType;
                basketItem.Brand = product.ProductBrand;
                basketItem.Quantity = 1;

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:5001/api/basket/getbasket/{basketID}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        output = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse);
                        //var ss = JsonConvert.DeserializeObject<List<BasketItem>>(_httpContextAccessor.HttpContext.Session.GetString("output"));
                        //output.Items = x;
                        var temp = output.Items.FindIndex(x=>x.Id == basketItem.Id);
                        if(temp == -1)
                        {
                            output.Items.Add(basketItem);
                        }
                        else
                        {
                            output.Items[temp].Quantity++;
                        }                                           

                        _httpContextAccessor.HttpContext.Session.SetString("output", JsonConvert.SerializeObject(output.Items));

                        var myContent = JsonConvert.SerializeObject(output);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                        using (var response1 = await httpClient.PostAsync("https://localhost:5001/api/basket/updatebasket", byteContent))
                        {
                            string apiResponse1 = await response1.Content.ReadAsStringAsync();
                            output = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse1);
                        }
                    }
                }               
            }
            if (output != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> FetchUserRole()
        {
            var userToken = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            var user = new UserDto();
            if (userToken != "" && userToken != null)
            {               
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                    using (var response = await httpClient.GetAsync("https://localhost:5001/api/account/getuser"))
                    {                       
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<UserDto>(apiResponse);
                    }
                }
                return user.Role;
            }
            return "";
        }


        public async Task<CustomerBasket> FetchBasket()
        {
            CustomerBasket basketProducts = new CustomerBasket();
            basketID = _httpContextAccessor.HttpContext.Session.GetString("basketId"); 
           
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:5001/api/basket/getbasket/{basketID}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    basketProducts = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse);
                }
            }

            return basketProducts;
        }

        public async Task<CustomerBasket> UpdateBasketMVC(CustomerBasketDto input)
        {
            using (var httpClient = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(input);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                using (var response = await httpClient.PostAsync("https://localhost:5001/api/basket/updatebasket", byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse);
                    _httpContextAccessor.HttpContext.Session.SetString("basketId", result.Id);
                    _httpContextAccessor.HttpContext.Session.SetString("output", JsonConvert.SerializeObject(result.Items));
                    return result;
                }                
            }            
        }   
    }
}
