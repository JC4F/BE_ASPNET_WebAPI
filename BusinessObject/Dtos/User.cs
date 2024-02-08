using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
