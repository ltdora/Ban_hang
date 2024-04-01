using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductService _productService;
        public ProductController(IProductService service)
        {
            _productService = service;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productService.GetProductsList();
                if (products == null) return NotFound();
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("[action]/id")]
        public IActionResult GetProductsById(int id)
        {
            try
            {
                var products = _productService.GetProductDetailsById(id);
                if (products == null) return NotFound();
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveProducts(Products productModel)
        {
            try
            {
                var model = _productService.SaveProduct(productModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var model = _productService.DeleteProduct(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }

}

