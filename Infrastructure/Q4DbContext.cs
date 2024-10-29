using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Q4DbContext : DbContext
{
    public Q4DbContext(DbContextOptions<Q4DbContext> options) : base(options)
    {
    }
}