using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Catedra3.src.Models
{
    public class AppUser : IdentityUser
    {

         public ICollection<Post> Posts { get; set; } = new List<Post>();
        
    }
}