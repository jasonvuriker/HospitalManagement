using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess;

public class HospitalContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }

    public DbSet<Doctor> Doctors { get; set; }

    public DbSet<Patient> Patients { get; set; }

    public PatientBlank PatientBlanks { get; set; }

    public Speciality Specialities { get; set; }


    public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(builder =>
        {
            builder.HasKey(r => r.AppointmentId);

            builder.HasOne(r => r.Patient)
                .WithMany(r => r.Appointments);

            builder.HasOne(r => r.Doctor)
                .WithMany(r => r.Appointments);
        });

        modelBuilder.Entity<Doctor>(builder =>
        {
            builder.HasKey(r => r.DoctorId);

            builder.HasOne(r => r.Speciality)
                .WithMany();
        });

        modelBuilder.Entity<Patient>(builder =>
        {
            builder.HasKey(r => r.PatientId);
        });

        modelBuilder.Entity<PatientBlank>(builder =>
        {
            builder.HasKey(r => r.PatientBlankId);

            builder.HasOne(r => r.Patient)
                .WithOne(r => r.PatientBlank)
                .HasForeignKey<PatientBlank>(r => r.PatientId);
        });

        modelBuilder.Entity<Speciality>(builder =>
        {
            builder.HasKey(r => r.SpecialityId);
        });
    }
}
