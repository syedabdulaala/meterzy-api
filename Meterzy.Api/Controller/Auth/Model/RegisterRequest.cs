using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller.Auth.Model
{
    public class RegisterRequest
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required, DataType(DataType.Text)]
        public string LastName { get; set; }
    }
}
