﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazingShop.Shared.Models;
using BlazingShop.Shared.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBlazingShopServices _blazingShopServices;

        public ProductController(IBlazingShopServices blazingShopServices)
        {
            _blazingShopServices = blazingShopServices;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetAllProducts()
        {
            var products = await _blazingShopServices.GetAllProductAsync();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProductById(int id)
        {
            var products = await _blazingShopServices.GetAllProductAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound($"Product with Id = {id} not found");
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Products product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
    
            await _blazingShopServices.AddProductAsync(product);

        // Assuming the ID is auto-generated by the database, returning the location of the newly created resource.
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Products product)
        {
           

            var existingProducts = await _blazingShopServices.GetAllProductAsync();
            if (!existingProducts.Any(p => p.Id == id))
            {
                return NotFound($"Product with Id = {id} not found");
            }

            await _blazingShopServices.UpdateProductAsync(product);
            return NoContent();  // 204 No Content, indicating successful update without returning data
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var products = await _blazingShopServices.GetAllProductAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound($"Product with Id = {id} not found");
            }

            await _blazingShopServices.DeleteProductAsync(id);
            return NoContent();  // 204 No Content, indicating successful deletion
        }

        [HttpGet("DistinctTitles")]
        public async Task<ActionResult<List<string>>> GetDistinctTitles()
        {
            try
            {
                var distinctTitles = await _blazingShopServices.GetDistinctTitlesAsync();
                return Ok(distinctTitles);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("DistinctDescription")]
        public async Task<ActionResult<List<string>>> GetDistinctDescription()
        {
            try
            {
                var DistinctDescription = await _blazingShopServices.GetDistinctDescriptionsAsync();
                return Ok(DistinctDescription);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




    }
}
