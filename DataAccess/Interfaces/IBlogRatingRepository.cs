using BusinessObject.Dtos;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IBlogRatingRepository: IRepositoryBase<BlogRating>
    {
        Task<IEnumerable<BlogRating>> GetAllBlogRating();
        Task<BlogRating?> FindBlogRatingByID(Guid id);
        Task<int> RatingBlog(CreateBlogRatingDto data, Guid userID);
        Task<int> CreateBlogRating(CreateBlogRatingDto data, Guid userID);
        Task<int> UpdateBlogRating(Guid id, Guid userID, UpdateBlogRatingDto data);
    }
}
