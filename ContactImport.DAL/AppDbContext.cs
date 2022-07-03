using ContactImport.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactImport.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ContactEntity> Contacts => Set<ContactEntity>();
}