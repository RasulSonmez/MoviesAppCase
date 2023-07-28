using Entities.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities.Models
{
    public class Movie:BaseEntity
    {
        [Display(Name = "Title")]
        [MaxLength(156, ErrorMessage = "Cannot be longer than 156 characters")]
        [Required(ErrorMessage = "Required field")]
        public string Title { get; set; }

        [Display(Name = "Summary")]
        [MaxLength(256, ErrorMessage = "Cannot be longer than 256 characters")]
        [Required(ErrorMessage = "Required field")]
        public string Summary { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000, ErrorMessage = "Cannot be longer than 1000 characters")]
        [Required(ErrorMessage = "Required field")]
        public string Description { get; set; }


        [Display(Name = "Release Date")]      
        [Required(ErrorMessage = "Required field")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Cover Image")]
        public string? CoverImageFilePath { get; set; }

        //not mapped
        [NotMapped]
        [Display(Name = "Cover Image")]      
        [Required(ErrorMessage = "Required field")]       
        public IFormFile CoverImageFile { get; set; }

        public int? MovieCategoryId { get; set; }

        public MovieCategory? MovieCategory { get; set; }
    }
}
