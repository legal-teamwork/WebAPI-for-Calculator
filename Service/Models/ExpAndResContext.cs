using Microsoft.EntityFrameworkCore;

namespace ServiceCalculator.Models;

public class ExpAndResContext : DbContext
{
    public ExpAndResContext(DbContextOptions<ExpAndResContext> options)
        : base(options)
    {
    }

    public DbSet<ExpAndResItem> ExpAndResItems { get; set; } = null!;
}
