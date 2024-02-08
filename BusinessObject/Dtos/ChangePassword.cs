using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dtos
{
    public class ChangePasswordRequestDto
    {
        [Required, EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }

    }
}
