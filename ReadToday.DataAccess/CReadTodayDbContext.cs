using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ReadToday.Model;

namespace ReadToday.DataAccess
{
    public class CReadTodayDbContext : DbContext
    {
        public CReadTodayDbContext() : base("ReadTodayDb")
        {

        }

        public DbSet<CBook> Books { get; set; }
        public DbSet<CLiteraryGenre> LiteraryGenres { get; set; }
        public DbSet<CCharacter> Characters { get; set; }
        public DbSet<CShelf> Shelfs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
