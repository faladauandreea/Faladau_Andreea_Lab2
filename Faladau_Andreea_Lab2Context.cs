
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

public class Faladau_Andreea_Lab2Context(
    DbContextOptions<Faladau_Andreea_Lab2Context> options
) : DbContext(options)
{
    public DbSet<Book> Book { get; set; } = default!;

    public DbSet<Publisher> Publisher { get; set; } = default!;

    public DbSet<Authors> Authors { get; set; } = default!;
}
