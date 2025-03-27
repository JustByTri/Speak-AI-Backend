using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class UpdateUserProfileDTO
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime? Birthday { get; set; }

        public string Gender { get; set; }

        public string? NewPassword { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
