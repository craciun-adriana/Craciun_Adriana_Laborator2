using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Craciun_Adriana_Laborator2.Models;

namespace Craciun_Adriana_Laborator2.Data
{
    public class Craciun_Adriana_Laborator2Context : DbContext
    {
        public Craciun_Adriana_Laborator2Context (DbContextOptions<Craciun_Adriana_Laborator2Context> options)
            : base(options)
        {
        }

        public DbSet<Craciun_Adriana_Laborator2.Models.Book> Book { get; set; } = default!;
        public DbSet<Craciun_Adriana_Laborator2.Models.Customer> Customer { get; set; } = default!;
        public DbSet<Craciun_Adriana_Laborator2.Models.Genre> Genre { get; set; }
        public DbSet<Craciun_Adriana_Laborator2.Models.Author> Author { get; set; }
    }
}
