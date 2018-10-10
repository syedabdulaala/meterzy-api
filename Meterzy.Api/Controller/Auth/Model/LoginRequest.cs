using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller.Auth.Model
{
    public sealed class LoginRequest
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Password { get; set; }
    }
}
