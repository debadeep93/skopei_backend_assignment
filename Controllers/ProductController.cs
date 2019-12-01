using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkopeiBackendAssignment.Entities;
using SkopeiBackendAssignment.Entities.IRepository;
using SkopeiBackendAssignment.Exceptions;

namespace SkopeiBackendAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // using the data manager interface to interact with the database
        private readonly IDataManager<Product> _product;

        public ProductController(IDataManager<Product> product)
        {
            // injecting into the controller context
            _product = product;
        }



        /// <summary>
        /// Returns a list of products
        /// </summary>
        /// <returns>List of Products</returns>
        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // invokes list retreival method through Data access interface object
            return Ok(await _product.GetAll());
        }

        /// <summary>
        /// Returns a specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The product denoted by provided id</returns>
        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            try
            {
                // invokes retreival method through Data access interface object
                var product = await _product.Get(id);
                return Ok(product);
            }
            // catches custom exception thrown by Data access layer
            catch(EntityNotFoundException e)
            {
                // returns 404 and message if product is not found
                return NotFound(e.Message);
            }

            
        }



        /// <summary>
        /// Updates a specific product and returns it for view
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>The product recently updated</returns>
        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            // id provided in uri path must match id in body data to proceed
            if (id != product.Id)
            {
                return BadRequest();
            }
            try
            {
                // invokes update method through Data access interface object
                var pr = await _product.Update(id, product);
                return Ok(pr);
            }
            // catches custom exception thrown by Data access layer
            catch (EntityNotFoundException e)
            {
                // returns 404 and message if product is not found
                return NotFound(e.Message);
            }
        }


        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The recently created product</returns>
        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // rounds any price data to 2 places after decimal to comply with currency
            product.Price = decimal.Round(product.Price, 2);

            // invokes add method through Data access interface object
            await _product.Add(product);

            // returns the recently added product
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// Deletes a specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The recently deleted product for view</returns>
        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            try
            {
                // invokes delete method through Data access interface object
                var product = await _product.Delete(id);
                return product;
            }
            // catches custom exception thrown by Data access layer
            catch (EntityNotFoundException e)
            {
                // returns 404 and message if product is not found
                return NotFound(e.Message);
            }
 
        }
    }
}
