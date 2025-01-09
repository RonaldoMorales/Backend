using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3.src.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Catedra3.src.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser>

    {

        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

            
        }
        
    }
}