using BusinessObject.Dtos;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserRepository: IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User?> FindUserByID(Guid id);
        Task<User?> FindUserByEmail(string email);
        Task<User?> FindUserByEmailAndPw(string email, string password);
        Task<int> CreateUser(RegisterRequestDto data);
        Task<int> UpdatePassword(ChangePasswordRequestDto data);
        Task<string> GenareateTokenForgotPassword(string email);
        Task<bool> VerifiedTokenForgotPasswordWithUpdate(VerifyForgotPasswordDto data);
    }
}
