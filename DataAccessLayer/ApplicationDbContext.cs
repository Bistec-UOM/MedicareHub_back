using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<LabReport> labReports { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Patient_Teles> Patient_Teles { get; set; }
        public DbSet<Record> records { get; set; }
        public DbSet<ReportFileds> reportFileds { get; set; }
        public DbSet<Test> tests { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<User_Tele> user_Teles { get; set; }

    }
}
