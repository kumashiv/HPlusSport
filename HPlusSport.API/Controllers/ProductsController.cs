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






    }
}
