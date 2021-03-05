using Core.Entities.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Webshop.Shared
{
    public class SharedSpace
    {
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
    }
}
