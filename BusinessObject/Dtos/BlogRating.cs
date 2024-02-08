using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class CreateBlogRatingDto
    {
        public VOTE Vote { get; set; }
        public Guid BlogID { get; set; }
    }

    public class UpdateBlogRatingDto
    {
        public VOTE Vote { get; set; }
    }

    public class BlogRatingDto
    {
        public Guid ID { get; set; }
        public VOTE Vote { get; set; }
        public Guid UserID { get; set; }
        public Guid BlogID { get; set; }
    }
}
