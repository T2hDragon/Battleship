using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(@"
                    Server=barrel.itcollege.ee,1533;
                    User id=student;
                    Password=Student.Bad.password.0;
                    MultipleActiveResultSets=true;
                    Database=kaalte_BattleshipDB");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}