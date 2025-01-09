using System;
using System.ComponentModel.DataAnnotations;

namespace Catedra3.src.Dto.Post
{
    public class CreatePostDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MinLength(5, ErrorMessage = "El título debe tener al menos 5 caracteres.")]
        public string Titulo { get; set; }
    }
}