using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class RegisterRequestDto
    {
        [Required, EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string RePassword { get; set; }

    }
}