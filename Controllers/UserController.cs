using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkopeiBackendAssignment.Entities;
using SkopeiBackendAssignment.Entities.IRepository;
using SkopeiBackendAssignment.Exceptions;

namespace SkopeiBackendAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // using the data manager interface to interact with the database
        private readonly IDataManager<User> _user;

        public UserController(IDataManager<User> user)
        {
            // injecting into the controller context
            _user = user;
        }



        /// <summary>
        /// Returns a list of users
        /// </summary>
        /// <returns>List of Users</returns>
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // invokes list retreival method through Data access interface object
            return Ok(await _user.GetAll());
        }

        /// <summary>
        /// Returns a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user denoted by provided id</returns>
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            try
            {
                // invokes retreival method through Data access interface object
                var user = await _user.Get(id);
                return Ok(user);
            }
            // catches custom exception thrown by Data access layer
            catch (EntityNotFoundException e)
            {
                // returns 404 and message if user is not found
                return NotFound(e.Message);
            }


        }



        /// <summary>
        /// Updates a specific user and returns it for view
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>The user recently updated</returns>
        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            // id provided in uri path must match id in body data to proceed
            if (id != user.Id)
            {
                return BadRequest();
            }
            try
            {
                // invokes update method through Data access interface object
                var pr = await _user.Update(id, user);
                return Ok(pr);
            }
            // catches custom exception thrown by Data access layer
            catch (EntityNotFoundException e)
            {
                // returns 404 and message if user is not found
                return NotFound(e.Message);
            }
        }


        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The recently created user</returns>
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // rounds any price data to 2 places after decimal to comply with currency
            if (!EmailIsValid(user.Email))
            {
                return BadRequest("Invalid Email Address Provided");
            }

            // invokes add method through Data access interface object
            await _user.Add(user);

            // returns the recently added user
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        

        /// <summary>
        /// Deletes a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The recently deleted user for view</returns>
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            try
            {
                // invokes delete method through Data access interface object
                var user = await _user.Delete(id);
                return user;
            }
            // catches custom exception thrown by Data access layer
            catch (EntityNotFoundException e)
            {
                // returns 404 and message if user is not found
                return NotFound(e.Message);
            }

        }

        /// <summary>
        /// Checks if the email entered by a user is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true | if email address is valid</returns>
        /// <returns>false | if email address is invalid</returns>
        private bool EmailIsValid(string email)
        {
            try
            {
                // uses System.Net.Mail class to validate email
                MailAddress maillAdd = new MailAddress(email);
                return true;
            }
            catch (FormatException ex)
            {
                // if format is invalid causes a format exception
                return false;
            }
        }
    }
}
