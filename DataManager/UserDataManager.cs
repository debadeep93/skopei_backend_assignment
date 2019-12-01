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
    public class UserDataManager : IDataManager<User>
    {
        // using EF Data context object to perform actions on DB
        private readonly TestDataContext _context;

        public UserDataManager(TestDataContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User user)
        {
            // adds a user to the db context
            _context.Add(user);
            // saves the changes, commiting them to the database
            await _context.SaveChangesAsync();

            // returns the recently added user
            return user;
        }

        public async Task<User> Delete(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User", id);
            }

            // updating audit fields
            user.Deleted = true;
            user.DateModified = DateTime.UtcNow;

            // soft delete
            // updates the audit field Deleted to true but does not remove the entry from db
            await Update(id, user);

            // returns the deleted user for view
            return user;
        }

        public async Task<User> Get(long id)
        {
            // fetches the user by id
            var user = await _context.Users.FindAsync(id);

            // if no user is found
            if (user == null)
            {
                // custom exception with message is thrown
                throw new EntityNotFoundException("User", id);
            }
            // returns the fetched user
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            // returns the list of all users in db context
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Update(long id, User user)
        {
            // alters the state to modified for saving of the entry user with current information
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                // save changes to the database
                await _context.SaveChangesAsync();

                // return the updated record for view
                return user;
            }
            // catches UpdateConcurrency Exception
            catch (DbUpdateConcurrencyException)
            {
                // if it is caused by user not found
                if (!UserExists(id))
                {
                    // throws custom exception with message
                    throw new EntityNotFoundException("User", id);
                }
                else
                {
                    // throws exception stack
                    throw;
                }
            }
        }

        // checks if the user by id exists
        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
