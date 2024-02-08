using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Category: BaseModel
    {
        public string Name { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }
        public string Status { get; set; }

    }
}
