using APICatalogo_MinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo_MinimalAPI.Context;
public class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto>? Produtos { get; set; }
    public DbSet<Categoria>? Categorias { get; set; }
}

