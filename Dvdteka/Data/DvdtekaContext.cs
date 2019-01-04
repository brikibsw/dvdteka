using Microsoft.EntityFrameworkCore;

namespace Dvdteka.Data
{
    public class DvdtekaContext : DbContext
    {
        public DvdtekaContext(DbContextOptions<DvdtekaContext> options) 
            : base(options)
        {
        }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Dvd> Dvds { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<MemberContact> MemberContacts { get; set; }

        public DbSet<Rent> Rents { get; set; }
    }
}
