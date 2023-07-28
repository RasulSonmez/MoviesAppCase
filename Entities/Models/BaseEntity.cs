using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities.Model
{
    public class BaseEntity
    {

        public int Id { get; set; }

        [Display(Name = "Create date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedById { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedAt { get; set; } = DateTime.Now;

        public string? ModifiedById { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
        public string? DeletedById { get; set; }

    }
}
