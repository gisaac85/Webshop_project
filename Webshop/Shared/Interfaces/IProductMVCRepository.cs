using Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Shared.Interfaces
{
    public interface IProductMVCRepository : IRepository<ProductToReturnDto>
    {
        Task<List<ProductToReturnDto>> GetListByNameAsync(string url, string name);
        Task<List<ProductToReturnDto>> GetListByIdAsync(string url, int id);
    }
}
