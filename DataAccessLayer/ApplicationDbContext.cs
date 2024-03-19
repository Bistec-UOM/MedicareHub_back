using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //Receptionist============================================================
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Unable_Date> unable_Dates { get; set; }

        //Doctor==================================================================
        public DbSet<Prescription> prescriptions { get; set; }
        public DbSet<Prescript_drug> prescript_Drugs { get; set; }

        //Pharmacy================================================================
        public DbSet<Drug> drugs { get; set; }
        public DbSet<Bill_drug> bill_Drugs { get; set; }

        //Lab=====================================================================
        public DbSet<LabReport> labReports { get; set; }
        public DbSet<Record> records { get; set; }
        public DbSet<Test> tests { get; set; }
        public DbSet<ReportFields> reportFields { get; set; }

        //Admin===================================================================
        public DbSet<Patient> patients { get; set; }
        public DbSet<Patient_Teles> Patient_Teles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<User_Tele> user_Teles { get; set; }
        //----------------------------------------------
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Receptionist> receptionists { get; set; }
        public DbSet<Cashier> cashiers { get; set; }
        public DbSet<LabAssistant> labAssistants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            // Configure other entity relationships

            base.OnModelCreating(modelBuilder);
        }

    }


}
