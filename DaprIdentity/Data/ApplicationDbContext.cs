using DaprIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace DaprIdentity.Data;

public class ApplicationDbContext : DbContext
{
    /// <inheritdoc />
    public ApplicationDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) { }
}
