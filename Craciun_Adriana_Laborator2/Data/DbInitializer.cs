using Microsoft.EntityFrameworkCore;
using Craciun_Adriana_Laborator2.Models;

namespace Craciun_Adriana_Laborator2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Craciun_Adriana_Laborator2Context(serviceProvider.GetRequiredService<DbContextOptions<Craciun_Adriana_Laborator2Context>>()))
            {
                if (context.Book.Any())
                {
                    return;   // BD a fost creata anterior
                }
                context.Book.AddRange(
                new Book { Title = "Baltagul", AuthorID = 1, Price = Decimal.Parse("22") },
                new Book { Title = "Enigma Otiliei", AuthorID = 2, Price = Decimal.Parse("18") },
                new Book { Title = "Maytrei", AuthorID = 3, Price = Decimal.Parse("27") }
                );

                context.Genre.AddRange(
                    new Genre { Name = "Roman" },
                    new Genre { Name = "Nuvela" },
                    new Genre { Name = "Poezie" }
                    );
                context.Customer.AddRange(
                    new Customer { Name = "Popescu Marcela", Adress = "Str. Plopilor, nr. 24", BirthDate = DateTime.Parse("1979-09-01") },
                    new Customer { Name = "Mihailescu Cornel", Adress = "Str. Bucuresti, nr. 45, ap. 2", BirthDate = DateTime.Parse("1969-07-08") }
                    );
                context.Author.AddRange(
                    new Author { Id = 1, FirstName = "Mihail", LastName = "Sadoveanu" },
                    new Author { Id = 2, FirstName = "George", LastName = "Calinescu" },
                    new Author { Id =3, FirstName = "Mircea", LastName = "Eliade" }
                    );
                context.SaveChanges();

            } 
        } 
    } 
}

