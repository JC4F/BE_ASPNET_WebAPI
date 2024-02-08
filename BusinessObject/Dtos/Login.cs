using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
