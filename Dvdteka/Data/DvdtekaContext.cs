using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dvdteka.Data
{
    public class DvdtekaContext : DbContext
    {
        private readonly ILoggerFactory loggerFactory;

        public DvdtekaContext(DbContextOptions<DvdtekaContext> options, ILoggerFactory loggerFactory) 
            : base(options)
        {
            this.loggerFactory = loggerFactory;
        }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Dvd> Dvds { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<MemberContact> MemberContacts { get; set; }

        public DbSet<Rent> Rents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(loggerFactory);
        }
    }
}
