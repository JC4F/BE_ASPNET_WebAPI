using BusinessObject.Dtos;
using BusinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DataAccess.Services
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(PRN231Context repositoryContext)
           : base(repositoryContext)
        {
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await FindAll().OrderBy(u => u.CreatedAt).ToListAsync();
        }

        public async Task<User?> FindUserByID(Guid id)
        {
            return await FindByCondition(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<User?> FindUserByEmail(string email)
        {
            return await FindByCondition(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> FindUserByEmailAndPw(string email, string password)
        {
            var user = await FindByCondition(x => (x.Email == email)).FirstOrDefaultAsync();

            if (user != null && VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return user;

            return null;
        }

        public async Task<int> CreateUser(RegisterRequestDto data)
        {

            CreatePasswordHash(data.Password,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);

            var user = new User
            {
                Email = data.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = ROLE.USER,
                Status = STATUS.ENABLE,
                CreatedAt = DateTime.UtcNow
            };

            Create(user);
            return await RepositoryContext.SaveChangesAsync();
        }

        public async Task<int> UpdatePassword(ChangePasswordRequestDto data)
        {
            if (!data.NewPassword.Equals(data.RePassword))
            {
                throw new Exception("Password not match");
            }

            var user = await DbSet.FirstOrDefaultAsync(x => (x.Email == data.Email));

            if (user == null || !VerifyPasswordHash(data.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Wrong old password");
            }

            CreatePasswordHash(data.NewPassword,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await RepositoryContext.SaveChangesAsync();
        }

        public async Task<string> GenareateTokenForgotPassword(string email)
        {
            var user = await DbSet.FirstOrDefaultAsync(x => (x.Email == email));
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var token = CreateRandomToken();
            user.VerificationToken = token;
            user.VerifiedAt = DateTime.Now.AddDays(1);
            await RepositoryContext.SaveChangesAsync();

            return token;
        }

        public async Task<bool> VerifiedTokenForgotPasswordWithUpdate(VerifyForgotPasswordDto data)
        {
            var user = await DbSet.FirstOrDefaultAsync(x => (x.Email == data.Email));
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!data.Password.Equals(data.RePassword))
            {
                throw new Exception("Password not match");
            }


            if (user.VerificationToken == data.Token && user.VerifiedAt > DateTime.Now)
            {
                CreatePasswordHash(data.Password,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.VerificationToken = null;
                user.VerifiedAt = null;

                await RepositoryContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
