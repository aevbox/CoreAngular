using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreAngular.Models;
using CoreAngular.Services;

namespace CoreAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationContext db;

        public ProductsController(ApplicationContext context)
        {
            db = context;
            if (!db.Products.Any())
            {
                db.Products.Add(new Product { Name = "iPhone X", Company = "Apple", Price = 79900 });
                db.Products.Add(new Product { Name = "Galaxy S8", Company = "Samsung", Price = 49900 });
                db.Products.Add(new Product { Name = "Pixel 2", Company = "Google", Price = 52900 });
                db.SaveChanges();
            }
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return db.Products.ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}