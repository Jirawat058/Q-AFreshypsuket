using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "ปัญหา")]
        [StringLength(1000, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "หมวดหมู่")]
        [StringLength(30)]
        public string Genre { get; set; }

        [Display(Name = "คำตอบ")]
        public string Rating { get; set; }

        [Display(Name = "สถานะ")]
        public string Status { get; set; }


    }

    public class MovieDBContext : DbContext {
        public DbSet<Movie> Movies { get; set; }
    }

}
