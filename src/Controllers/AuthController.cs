using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Catedra3.src.Dto.Auth;
using Catedra3.src.Models;
using Catedra3.src.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Catedra3.Controllers
{

    [Route("catedra3/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {



        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        
        private readonly SignInManager<AppUser> _signInManager;


        public AuthController(UserManager<AppUser> userManager, TokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager; 
            _tokenService = tokenService;
            _signInManager = signInManager;
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

                 var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "El correo electrónico ya está registrado." });
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    return Unauthorized(new { Message = "Credenciales inválidas." });
                }

                var token = _tokenService.CreateToken(user);

                var newUser = new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = token
                };

                return Ok(newUser);
            }

            catch(DbException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}