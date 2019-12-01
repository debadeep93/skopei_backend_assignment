using Microsoft.EntityFrameworkCore;
using SkopeiBackendAssignment.DataContext;
using SkopeiBackendAssignment.Entities;
using SkopeiBackendAssignment.Entities.IRepository;
using SkopeiBackendAssignment.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkopeiBackendAssignment.DataManager
{
    public class ProductDataManager : IDataManager<Product>
    {
        // using EF Data context object to perform actions on DB
        private readonly TestDataContext _context;

        public ProductDataManager(TestDataContext context)
        {
            _context = context;
        }

        public async Task<Product> Add(Product product)
        {
            // adds a product to the db context
             _context.Add(product);
            // saves the changes, commiting them to the database
            await _context.SaveChangesAsync();

            // returns the recently added product
            return product;
        }

        public async Task<Product> Delete(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new EntityNotFoundException("Product", id);
            }

            // updating audit fields
            product.Deleted = true;
            product.DateModified = DateTime.UtcNow;

            // soft delete
            // updates the audit field Deleted to true but does not remove the entry from db
            await Update(id, product);

            // returns the deleted product for view
            return product;
        }

        public async Task<Product> Get(long id)
        {
            // fetches the product by id
            var product = await _context.Products.FindAsync(id);

            // if no product is found
            if (product == null)
            {
                // custom exception with message is thrown
                throw new EntityNotFoundException("Product", id);
            }
            // returns the fetched product
            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            // returns the list of all products in db context
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> Update(long id, Product product)
        {
            // alters the state to modified for saving of the entry product with current information
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                // save changes to the database
                await _context.SaveChangesAsync();

                // return the updated record for view
                return product;
            }
            // catches UpdateConcurrency Exception
            catch (DbUpdateConcurrencyException)
            {
                // if it is caused by product not found
                if (!ProductExists(id))
                {
                    // throws custom exception with message
                    throw new EntityNotFoundException("Product", id);
                }
                else
                {
                    // throws exception stack
                    throw;
                }
            }
        }

        // checks if the product by id exists
        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
