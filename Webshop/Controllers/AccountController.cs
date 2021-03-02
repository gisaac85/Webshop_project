﻿using Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Webshop.SharedMethods;

namespace Webshop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult UserRegister()
        {
            return View("UserRegister");
        }


        [HttpPost]
        public async Task<IActionResult> UserLogin(LoginDto login)
        {
            UserDto result = new UserDto();
            using (var httpClient = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(login);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                using (var response = await httpClient.PostAsync("https://localhost:5001/api/account/login",byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UserDto>(apiResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Products", null);
                    }
                    else
                    {
                        TempData["loginMessage"] = "Email or Password not correct!";
                        return View("Index");
                    }
                }
            }          
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(RegisterDto user)
        {
            UserDto result = new UserDto();
            using (var httpClient = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(user);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                using (var response = await httpClient.PostAsync("https://localhost:5001/api/account/register", byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UserDto>(apiResponse);
                }
            }
            if (result != null)
            {
                return RedirectToAction("Index", "Products", null);
            }
            else
            {
                return View("Index");
            }
        }
    }
}
