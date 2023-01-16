using Microsoft.EntityFrameworkCore;
using OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Personel { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<DiseaseDoctor> DiseaseDoctors { get; set; }
        public DbSet<DiseasePatient> DiseasePatients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentNote> AppointmentNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "project.db")}");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title>().HasData(
                new Title { Id = 1, PersonTitle = "Sekreter", Status = true },
                new Title { Id = 2, PersonTitle = "Doktor", Status = true },
                new Title { Id = 3, PersonTitle = "Hasta", Status = true });
        }
    }
}
