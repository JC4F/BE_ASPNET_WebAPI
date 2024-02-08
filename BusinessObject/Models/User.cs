using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public static class STATUS
    {
        public const string ENABLE = "Enable";
        public const string DISABLE = "Disable";
        public const string PENDING = "Pending";
    }

    public static class ROLE
    {
        public const string ADMIN = "Admin";
        public const string SELLER = "Seller";
        public const string USER = "User";
    }

    public class User: BaseModel
    {
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<BlogRating> BlogRatings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Whislist> Whislists { get; set; }
    }
}
