using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        //Start POST (create)
        // api/Restaurant

        [HttpPost] //by using [HttpPost] we can name CreateRestaurant anything and it will know its a Post method.
        public async Task<IHttpActionResult> CreateRestaurant([FromBody] Restaurant model)
        {
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            //if the model is valid
            if (ModelState.IsValid)
            {
                //store the model in the database
                _context.Restaurants.Add(model);
                //this will save and count all changes made
                int changeCount = await _context.SaveChangesAsync();
                return Ok("Your restaurant was created!");
            }

            //the model is not valid, go ahead and rejecty it
            return BadRequest(ModelState);
        }

        //Get All
        //api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }


        //Get By ID
        //api/Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }

        //PUT (update)
        //api/Restaurant/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            //Check the ids if the match
            if (id != updatedRestaurant?.Id)
            {
                return BadRequest("Ids do not match.");
            }
            //check the ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Find the restaurant in the database
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            //If the restaurant doesnt exist then do something
            if (restaurant is null)
                return NotFound();


            //Update the properties
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;
            


            //save the changes
            await _context.SaveChangesAsync();
            return Ok("The restaurant was updated!");


        }

        // DELETE (delete)
        // api/Restaurant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant is null)
            {
                return NotFound();

            }
                _context.Restaurants.Remove(restaurant);

                if (await _context.SaveChangesAsync() == 1)
                {
                    return Ok("The restaurant was deleted");
                }

                return InternalServerError();
        }



    }
}
