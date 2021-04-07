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
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        // Create new ratings
        // POST api/Rating
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating([FromBody] Rating model)
        {
            //Check if model is null
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty");
            }
            //Check is model is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Find the Restaurant by model.RestaurantId and see that it exists
            var restaurantEntity = await _context.Restaurants.FindAsync(model.RestaurantId);
            if (restaurantEntity is null)
                return BadRequest($"The target restaurant with the ID of {model.RestaurantId} does not exist.");

            // Create the Rating

            // Add to the Rating table
            //_context.Ratings.Add(model);

            //Add to the Restaurant Entity
            restaurantEntity.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
                return Ok($"You rated restaurant {restaurantEntity.Name} successfully!");

            return InternalServerError();
        }
        // Get a rating by its ID
        [HttpGet]
        public async Task<IHttpActionResult> GetByRatingId([FromUri] int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);

            if (rating != null)
            {
                return Ok(rating);
            }

            return NotFound();
        }


        // Get All Ratings
        [HttpGet]
        public async Task<IHttpActionResult> GetAllRatings()
        {
            List<Rating> rating = await _context.Ratings.ToListAsync();
            return Ok(rating);
        }

        // Get All Ratings for a specific restaurant by the Restaurant ID


        //Update a Rating
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRating([FromUri] int id, [FromBody] Rating updatedRating)
        {
            
            if (id != updatedRating?.Id)
            {
                return BadRequest("Ids do not match.");
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            Rating rating = await _context.Ratings.FindAsync(id);
            
            if (rating is null)
                return NotFound();


            
            rating.FoodScore = updatedRating.FoodScore;
            rating.CleanlinessScore = updatedRating.CleanlinessScore;
            rating.EnvironmentScore = updatedRating.EnvironmentScore;



            
            await _context.SaveChangesAsync();
            return Ok("The restaurant was updated!");


        }

        // Delete a Rating
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRating([FromUri] int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);

            if (rating is null)
            {
                return NotFound();

            }
            _context.Ratings.Remove(rating);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The rating was deleted");
            }

            return InternalServerError();
        }
    }
}
