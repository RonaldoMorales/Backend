using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Catedra3.src.Data;
using Catedra3.src.Dto.Post;
using Catedra3.src.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Catedra3.src.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        private readonly Cloudinary _cloudinary; 

        private string? _cloudinaryUrl ;


        public PostController(ApplicationDbContext context, IConfiguration configuration)
        {

            _cloudinaryUrl = configuration.GetSection("Cloudinary").GetSection("URL").Value;
            _context = context;
        
        }


        [HttpPost("create")]
        [Authorize]

        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto createPostDto, IFormFile imageFile)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = _context.Posts.Any(p => p.Titulo == createPostDto.Titulo);

            if(exists)
            {
                return BadRequest("El post ya existe");
            }

            string uploadedImageUrl = string.Empty;

            if(imageFile != null)
            {
                var allowedFormats = new[] {"image/jpg", "image/png"};
                if(!allowedFormats.Contains(imageFile.ContentType))
                {
                    return BadRequest("solo se admiten archivos jpg y png");
                }

                if(imageFile.Length > 5* 1024 * 1024)
                {
                    return BadRequest("El archivo no puede ser superior a 5MB");
                }

                try
                {
                    Cloudinary cloudinary = new Cloudinary(_cloudinaryUrl);
                    using var stream = imageFile.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(imageFile.FileName, stream),
                        UseFilename = true,
                        Overwrite = true, 
                        Folder = "catedra3"
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    uploadedImageUrl = uploadResult.SecureUrl?.ToString();

                    if(string.IsNullOrEmpty(uploadedImageUrl))
                    {
                        return BadRequest("No se pudo subir la imagen");
                    }




                }

                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            else
            {
                return BadRequest("La imagen es requerida");
            }

             var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var newPost = new Post
            {
                Titulo = createPostDto.Titulo,
                LinkImagen = uploadedImageUrl,
                Fecha = DateTime.Now,
                UserId = userId
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return Ok("Post subido exitosamente");

        }

        [HttpGet("my-posts")]
        [Authorize]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado o no se encontró el ID del usuario en el token.");
            }

            var userPosts = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (userPosts == null || userPosts.Count == 0)
            {
                return NotFound("No se encontraron posts para este usuario.");
            }

            return Ok(userPosts);
        }


        
    }
}