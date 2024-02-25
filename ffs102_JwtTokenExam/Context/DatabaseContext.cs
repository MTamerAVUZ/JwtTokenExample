using ffs102_JwtTokenExam.Models;
using Microsoft.EntityFrameworkCore;

namespace ffs102_JwtTokenExam.Context
{
  public class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<TokenModel> Token { get; set; }
  }
}
