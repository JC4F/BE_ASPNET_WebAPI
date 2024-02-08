using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Blog: BaseModel 
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int Seen { get; set; }
        //public int Helpful { get; set; }
        public string Status { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<BlogRating> BlogRatings { get; set; }
    }
}
