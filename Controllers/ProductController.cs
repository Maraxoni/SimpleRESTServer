using Microsoft.AspNetCore.Mvc;
using SimpleRESTServer.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using System.Globalization;

namespace SimpleRESTServer.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        // In-memory data storage using product name as key (assuming unique)
        private static readonly Dictionary<string, Product> _products = new Dictionary<string, Product>
        {
            { "Telefon", new Product { Name = "Telefon", Price = 1999.99, Producer = "Samsung" } },
            { "Laptop", new Product { Name = "Laptop", Price = 3499.50, Producer = "Dell" } }
        };

        // GET /api/products
        [HttpGet]
        public ActionResult<List<Product>> GetAll([FromQuery(Name = "startswith")] string? startsWith = null)
        {
            var result = _products.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(startsWith))
            {
                result = result.Where(p => p.Name.StartsWith(startsWith, true, CultureInfo.InvariantCulture));
            }

            return result.ToList();
        }

        // GET /api/products/{name}
        [HttpGet("{name}")]
        public ActionResult<Product> Get(string name)
        {
            if (_products.TryGetValue(name, out var product))
                return Ok(product);

            return NotFound();
        }

        // POST /api/products
        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            if (product?.Name == null)
                return BadRequest("Product must have a name.");

            if (_products.ContainsKey(product.Name))
                return Conflict("A product with the same name already exists.");

            _products[product.Name] = product;

            return CreatedAtAction(nameof(Get), new { name = product.Name }, product);
        }

        // PUT /api/products/{name}
        [HttpPut("{name}")]
        public ActionResult<Product> Update(string name, [FromBody] Product updatedProduct)
        {
            if (!_products.ContainsKey(name))
                return NotFound();

            if (updatedProduct?.Name != name)
                return BadRequest("Product name in URL and body must match.");

            _products[name] = updatedProduct;

            return Ok(updatedProduct);
        }

        // DELETE /api/products/{name}
        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            if (_products.Remove(name))
                return NoContent();

            return NotFound();
        }

        [HttpGet("params/{*any}")]
        public IActionResult TestParams(
            [FromQuery] string producer,
            [FromHeader(Name = "User-Agent")] string userAgent)
        {
            var path = Request.Path.Value;

            var matrixPart = path?.Split("params", 2).LastOrDefault();
            var matrixParams = matrixPart?.Split(';')
                .Skip(1)
                .Select(p => p.Split('='))
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0], p => p[1]);

            var absoluteUrl = Request.GetDisplayUrl();

            return Ok(new
            {
                FromQuery = producer,
                FromHeader = userAgent,
                MatrixParams = matrixParams,
                AbsoluteUrl = absoluteUrl,
                AllHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
            });
        }
    }
}
