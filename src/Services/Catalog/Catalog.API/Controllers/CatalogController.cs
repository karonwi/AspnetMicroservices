using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepo, ILogger<CatalogController> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          var products = await _productRepo.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> GetProductById(string Id)
        {
            var getProduct = await _productRepo.GetProductById(Id);
            if (getProduct == null)
            {
                _logger.LogError($"Product with id {Id} is not found");
                return NotFound();
            }
            return Ok(getProduct);
        }

        //the action would be the method name 
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var product = await _productRepo.GetProductByCategory(category);
            if (product == null)
            {
                _logger.LogError($"The category {category} does not exist");
                return NotFound();
            }
             return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepo.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(type: typeof(Product),(int)HttpStatusCode.OK)]
        //since we are not returning any particular type of result ,we 
        //can just return the IActionResult
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepo.UpdateProduct(product));
        }

        [HttpDelete]
        [ProducesResponseType(type: typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _productRepo.DeleteProduct(id));
        }
    }
}
