using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Catedra3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;


        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager; 
        }
        
    }
}