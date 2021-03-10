using AutoMapper;
using Core.Dtos;
using Core.Entities.OrderModels;
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
    public class CheckoutController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CheckoutController(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<Tuple<object, object, object, object>> PublicMethods()
        {
            var service = new SharedSpace(_httpContextAccessor);
            TempData["types"] = await service.FetchProductTypes();
            TempData["brands"] = await service.FetchProducBrands();
            var basketProducts = await service.FetchBasket();
            TempData["basketItems"] = basketProducts.Items.Count;
            decimal total = 0;
            foreach (var pro in basketProducts.Items)
            {
                total = total + (pro.Price * pro.Quantity);
                TempData["total"] = total;
            }
            return Tuple.Create(TempData["types"], TempData["brands"], TempData["basketItems"], TempData["total"]);
        }

        public async Task<IActionResult> GetAllOrdersForUser()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            var basketId = _httpContextAccessor.HttpContext.Session.GetString("basketId");

            if (token == null || token == "")
            {
                TempData["NotLoggedin"] = "You must loggedIn ...";
                return RedirectToAction("Index", "Account");
            }

            var result = new List<OrderToReturnDto>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/orders/getorders"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<OrderToReturnDto>>(apiResponse);                   
                }
            }
            await PublicMethods();
            return View("AllOrders",result);
        }

      
        public async Task<IActionResult> CheckoutBasket()
        {           
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            var basketId = _httpContextAccessor.HttpContext.Session.GetString("basketId");

            if (token == null || token == "")
            {
                TempData["NotLoggedin"] = "You must loggedIn ...";
                return RedirectToAction("Index", "Account");
            }

            OrderDto model = new OrderDto();
            AddressUserDto address = new AddressUserDto();
            var createdOrder = new Order();
            var deliveryMethods = new List<DeliveryMethod>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/account/address"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    address = JsonConvert.DeserializeObject<AddressUserDto>(apiResponse);
                }
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/orders/deliverymethods"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    deliveryMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(apiResponse);
                    ViewData["methods"] = deliveryMethods;
                }
            }


            model.BasketId = basketId;
            model.DeliveryMethodId = 4;
            model.ShipToAddress = new AddressDto();
            model.ShipToAddress.City = address.City;
            model.ShipToAddress.FirstName = address.FirstName;
            model.ShipToAddress.LastName = address.LastName;
            model.ShipToAddress.State = address.State;
            model.ShipToAddress.Street = address.Street;
            model.ShipToAddress.Zipcode = address.Zipcode;
         
            using (var httpClient = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(model);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync("https://localhost:5001/api/orders/createOrder", byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    createdOrder = JsonConvert.DeserializeObject<Order>(apiResponse);
                }
            }
            var result = _mapper.Map<Order, OrderToReturnDto>(createdOrder);         
            await PublicMethods();
            return View("Index", result);
        }

        public async Task<IActionResult> UpdateOrderMVC(OrderToUpdateDto input)
        {          
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            var result = new OrderToReturnDto();

            using (var httpClient = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(input);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync("https://localhost:5001/api/orders/updateOrder", byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrderToReturnDto>(apiResponse);
                }
            }

            return View("Index", result);
        }
    }
}
