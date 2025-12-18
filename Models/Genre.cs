using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craciun_Adriana_Laborator2.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
