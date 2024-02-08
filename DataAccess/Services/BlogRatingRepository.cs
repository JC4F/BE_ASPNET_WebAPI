using BusinessObject.Dtos;
using BusinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Services
{
    public class BlogRatingRepository : RepositoryBase<BlogRating>, IBlogRatingRepository
    {
        public BlogRatingRepository(PRN231Context repositoryContext)
           : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<BlogRating>> GetAllBlogRating()
        {
            return await FindAll().OrderBy(u => u.CreatedAt).ToListAsync();
        }

        public async Task<BlogRating?> FindBlogRatingByID(Guid id)
        {
            return await FindByCondition(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> RatingBlog(CreateBlogRatingDto data, Guid userID)
        {
            var BlogRating = await DbSet.FirstOrDefaultAsync(x => x.BlogID == data.BlogID && x.UserID == userID);

            if (data.Vote == VOTE.DEFAULT)
            {
                if (BlogRating != null)
                {
                    Delete(BlogRating);
                    return await RepositoryContext.SaveChangesAsync();
                }

                throw new Exception("Some thing wrong when save rating!");
            }
            else
            {
                if (BlogRating != null)
                {
                    BlogRating.Vote = data.Vote;
                    BlogRating.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var NewBlogRating = new BlogRating
                    {
                        Vote = data.Vote,
                        BlogID = data.BlogID,
                        UserID = userID,
                        CreatedAt = DateTime.UtcNow
                    };

                    Create(NewBlogRating);
                }
                return await RepositoryContext.SaveChangesAsync();
            }
        }

        public async Task<int> CreateBlogRating(CreateBlogRatingDto data, Guid userID)
        {
            var BlogRating = new BlogRating
            {
                Vote = data.Vote,
                BlogID = data.BlogID,
                UserID = userID,
                CreatedAt = DateTime.UtcNow
            };

            Create(BlogRating);
            return await RepositoryContext.SaveChangesAsync();
        }

        public async Task<int> UpdateBlogRating(Guid id, Guid userID, UpdateBlogRatingDto data)
        {
            var BlogRating = await DbSet.FirstOrDefaultAsync(x => x.ID == id && x.UserID == userID);
            if (BlogRating == null) return 0;

            if (data.Vote == VOTE.DEFAULT)
                Delete(BlogRating);
            else
            {
                BlogRating.Vote = data.Vote;
                BlogRating.UpdatedAt = DateTime.UtcNow;
            }

            return await RepositoryContext.SaveChangesAsync();
        }
    }
}
