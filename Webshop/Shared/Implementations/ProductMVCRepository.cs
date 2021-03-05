using Core.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Webshop.Shared.Interfaces;

namespace Webshop.Shared.Implementations
{
    public class ProductMVCRepository : Repository<ProductToReturnDto>,IProductMVCRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductMVCRepository(IHttpClientFactory clientFactory):base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<ProductToReturnDto>> GetListByNameAsync(string url, string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + name);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProductToReturnDto>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ProductToReturnDto>> GetListByIdAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProductToReturnDto>>(jsonString);
            }
            else
            {
                return null;
            }
        }
    }
}
