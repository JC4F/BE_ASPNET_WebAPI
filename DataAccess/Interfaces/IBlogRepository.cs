using BusinessObject.Dtos;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IBlogRepository: IRepositoryBase<Blog>
    {
        Task<IEnumerable<Blog>> GetAllBlog(string? role, string? userID);
        Task<IEnumerable<Blog>> GetAllPendingBlog();
        Task<Blog?> FindBlogByID(Guid id, string? role, string? userID);
        Task<int> CreateBlog(CreateBlogDto data, Guid UserID);
        Task<int> UpdateBlog(Guid id, Guid UserID, string role,  UpdateBlogDto data);
        Task<int> DeleteBlog(Guid id, Guid userID, string role);
    }
}
