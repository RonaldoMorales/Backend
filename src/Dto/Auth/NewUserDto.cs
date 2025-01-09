using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra3.src.Dto.Auth
{
    public class NewUserDto
    {

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!; 
        
    }
}