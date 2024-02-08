using BusinessObject.Dtos;
using BusinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Services
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(PRN231Context repositoryContext)
           : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Blog>> GetAllBlog(string? role, string? userID)
        {
            if (userID == null || role == null) return await DbSet.Where(x => x.Status == STATUS.ENABLE).Include(x => x.User).Include(x => x.BlogRatings).OrderBy(u => u.CreatedAt).ToListAsync();

            switch (role)
            {
                case ROLE.ADMIN:
                    return await DbSet.OrderBy(u => u.CreatedAt).ToListAsync();

                default:
                    return await DbSet.Where(x => (x.Status == STATUS.ENABLE || x.UserID == new Guid(userID))).Include(x => x.User).Include(x => x.BlogRatings).OrderBy(u => u.CreatedAt).ToListAsync();
            }
        }

        public async Task<IEnumerable<Blog>> GetAllPendingBlog()
        {
            return await DbSet.Where(x => x.Status == STATUS.PENDING).Include(x => x.User).Include(x => x.BlogRatings).OrderBy(u => u.CreatedAt).ToListAsync();
        }

        public async Task<Blog?> FindBlogByID(Guid id, string? role, string? userID)
        {
            if (userID == null || role == null) return await DbSet.Where(x => (x.Status == STATUS.ENABLE && x.ID == id)).Include(x => x.User).Include(x => x.BlogRatings).FirstOrDefaultAsync();

            switch (role)
            {
                case ROLE.ADMIN:
                    return await DbSet.Where(x => x.ID == id).Include(x => x.User).Include(x => x.BlogRatings).FirstOrDefaultAsync();

                default:
                    return await DbSet.Where(x => ((x.Status == STATUS.ENABLE || x.UserID == new Guid(userID)) && x.ID == id)).Include(x => x.User).Include(x => x.BlogRatings).FirstOrDefaultAsync();
            }
        }


        public async Task<int> CreateBlog(CreateBlogDto data, Guid UserID)
        {
            var Blog = new Blog
            {
                Title = data.Title,
                Content = data.Content,
                ImageUrl = data.ImageUrl,
                UserID = UserID,
                Status = STATUS.PENDING,
                CreatedAt = DateTime.UtcNow
            };

            Create(Blog);
            return await RepositoryContext.SaveChangesAsync();
        }

        public async Task<int> UpdateBlog(Guid id, Guid UserID, string role, UpdateBlogDto data)
        {
            var blog = await DbSet.FirstOrDefaultAsync(x => x.ID == id && x.UserID == UserID);
            if (blog == null)
            {
                throw new Exception("Blog not found");
            }

            if (data.Content != null)
                blog.Content = data.Content;
            if (data.Title != null)
                blog.Title = data.Title;
            if (data.ImageUrl != null)
                blog.ImageUrl = data.ImageUrl;
            if (data.Status != null)
            {
                if (data.Status == STATUS.ENABLE && role == ROLE.ADMIN) blog.Status = STATUS.ENABLE;
                else if (role != ROLE.ADMIN) blog.Status = STATUS.PENDING;

                else throw new Exception("Permission Denied");
            }
            if (data.Seen != null)
                blog.Seen = (int)data.Seen;
            blog.UpdatedAt = DateTime.UtcNow;

            return await RepositoryContext.SaveChangesAsync();
        }

        public async Task<int> DeleteBlog(Guid id, Guid userID, string role)
        {
            var blog = await DbSet.FirstOrDefaultAsync(x => (x.ID == id && (x.UserID == userID || role == ROLE.ADMIN)));
            if (blog == null || blog.Status == STATUS.DISABLE)
            {
                throw new Exception("Blog not found");
            }

            blog.Status = STATUS.DISABLE;
            return await RepositoryContext.SaveChangesAsync();
        }
    }
}
