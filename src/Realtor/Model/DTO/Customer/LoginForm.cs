using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.DTO.Customer
{
    public class LoginForm
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
    }
}
