using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Catedra3.src.Dto.Auth;
using Catedra3.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        [HttpPost("register")]


        public async Task<IActionResult> Register( [FromBody] RegisterDto registerDto)
        {

            try 
            {

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var AppUser = new AppUser
                {
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };


                var createUser = await _userManager.CreateAsync(AppUser, registerDto.Password);

                if(createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok("Usuario creado, no hubo problemas");
                    }
                    else
                    {
                        return StatusCode(500, "Error al asignar rol");
                    }
                    
                   
                }

                else
                {
                    return StatusCode(500, "Error al crear usuario");
                }

            }

            catch(DbException ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        
    }
}