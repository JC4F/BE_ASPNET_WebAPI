using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public enum VOTE
    {
        UNHELPFUL = -1, DEFAULT, HELPFUL
    }
    public class BlogRating: BaseModel
    {
        public VOTE Vote { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        [ForeignKey("Blog")]
        public Guid BlogID { get; set; }
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
