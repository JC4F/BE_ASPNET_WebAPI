using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class CreateBlogDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        //public Guid UserID { get; set; }
    }

    public class UpdateBlogDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? Seen { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class BlogDto
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int Seen { get; set; }
        public string Status { get; set; }
        public Guid UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserDto User;
        public List<BlogRatingDto> BlogRatings;
    }
}
