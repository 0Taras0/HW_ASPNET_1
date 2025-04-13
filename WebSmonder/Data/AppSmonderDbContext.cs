using Microsoft.EntityFrameworkCore;
using WebSmonder.Data.Entities;

namespace WebSmonder.Data;

public class AppSmonderDbContext : DbContext
{
    public AppSmonderDbContext(DbContextOptions<AppSmonderDbContext> opt) : base(opt) { }

    public DbSet<CategoryEntity> Categories { get; set; }
}
