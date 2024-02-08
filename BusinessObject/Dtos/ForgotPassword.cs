using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class VerifyForgotPasswordDto
    {
        [Required, EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    public class TokenRequestDto
    {
        [Required, EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
    }
}
