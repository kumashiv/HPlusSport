using HPlusSport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)      //Constructor
        {
            _context = context;

            _context.Database.EnsureCreated();  // To call seed method for data in models
        }


        [HttpGet]
        //public string GetAllProducts()
        //public ActionResult GetAllProducts()
        public async Task<ActionResult> GetAllProducts()     // It can returns products and also Http status code
        {
            return Ok(await _context.Products.ToArrayAsync());
        }

        //[HttpGet]
        //public IEnumerable<Product> GetAllProducts()
        //// IEnumerable can returns only a list of product - can't return Http status code
        //{
        //    return _context.Products.ToArray();
        //}


        //OR [HttpGet("{id}")]

        [HttpGet]
        [Route("{id}")]         //appending to Route on line 7 i.e. [Route("api/[controller]")]
        //public ActionResult GetProduct(int id)  
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAvailableProducts()
        {
            return await _context.Products.Where(p => p.IsAvailable).ToListAsync();   //OR
            //return await _context.Products.Where(p => p.IsAvailable).ToArrayAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            //201 Created Response if succesfull
            return CreatedAtAction(
                nameof(GetProduct),     // avoiding string "GetProduct"
                new { id = product.Id},
                product);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! _context.Products.Any(p => p.Id  == id))
                {
                    return NotFound();
                }else
                {
                    throw;
                }
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        //Delete Multiple
        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                products.Add(product);
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }


    }
}
