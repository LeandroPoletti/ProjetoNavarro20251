using Microsoft.EntityFrameworkCore;

namespace Pedido.Infra;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options)  :base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjetoEntidades.Models.Pedido>().HasKey(x => x.Id);
        
    }
}