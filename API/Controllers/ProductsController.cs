using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Entities;
using Core.Entities.ProductModels;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productsRepo;      

        public ProductsController(IProductRepository productsRepo)
        {        
            _productsRepo = productsRepo;
        }

        [HttpGet]
        [Route("getallproducts")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {          
            var products = await _productsRepo.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("getproduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productsRepo.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet]
        [Route("getproductbyname/{name}")]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            var product = await _productsRepo.GetProductByNameAsync(name);
            return Ok(product);
        }

        [HttpGet]
        [Route("getbrands")]
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _productsRepo.GetProductBrandsAsync();
        }

        [HttpGet]
        [Route("gettypes")]
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _productsRepo.GetProductTypesAsync();
        }

    }
}
