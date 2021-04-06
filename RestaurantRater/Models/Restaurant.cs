using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantRater.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public virtual List<Rating> Ratings { get; set; } = new List<Rating>();


        //[Required] we removed this after we created the Rating class.
        public double Rating
        {
            get
            {
                double totalAverageRating = 0;

                // add all ratings
                foreach (var rating in Ratings)
                {
                    totalAverageRating += rating.AverageRating;
                }

                // get average from total       
                return Ratings.Count > 0
                    ? Math.Round(totalAverageRating / Ratings.Count, 2)  // If Rating.Count > 0, it will display rounded to 2 points with Math.Round(totalAverageRating / Ratings.Count, 2)
                    : 0; // If Rating.Count is not > 0 
            }
        }

        public bool IsRecommended
        {
            get
            {
                return Rating > 8;
            }
        }

        //Average Food Rating

        // Average Cleanliness Rating

        // Average Environment Rating

    }
}