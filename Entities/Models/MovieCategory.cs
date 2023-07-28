using Entities.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities.Models
{
    public class MovieCategory:BaseEntity
    {
        [Display(Name = "Name")]
        [MaxLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        [Required(ErrorMessage = "Required field")]
        public string Name { get; set; }
    }
}
