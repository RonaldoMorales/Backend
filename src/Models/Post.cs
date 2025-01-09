using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra3.src.Models
{
    public class Post
    {


        public int Id { get; set; } // Primary Key

        public string Titulo { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;
        public string LinkImagen { get; set; } = string.Empty;

          // Relación con el usuario
        public string UserId { get; set; }
        public AppUser User { get; set; } // Navigation property
    }
}