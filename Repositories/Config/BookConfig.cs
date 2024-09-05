using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace WebApı.Repositories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "Karagöz ile Hacivat", Price = 55 },
                new Book { Id = 2, Title = "Mesnevi", Price = 587 },
                new Book { Id = 3, Title = "Simyacı", Price = 32 }
                );
        }
    }
}
