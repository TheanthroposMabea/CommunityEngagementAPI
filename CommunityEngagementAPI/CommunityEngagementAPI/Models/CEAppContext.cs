using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityEngagementAPI.Models

{
    public class CEAppContext : DbContext
    {
        public CEAppContext() { }

        public CEAppContext(DbContextOptions<CEAppContext> options) : base(options) { }


        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Sdg> Sdg { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
       
        public virtual DbSet<Alumni> Alumni { get; set; }
        public virtual DbSet<Campus> Campus { get; set; }
        public virtual DbSet<User_Demographics> User_Demographics { get; set; }
        public virtual DbSet<Project_Sdg> VolunteerSdg { get; set; }
        public virtual DbSet<Volunteer> Volunteer { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<NGO_Application> NGO_Application { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<NGO_Partnership> NGO_Partnership { get; set; }
        public virtual DbSet<Project_Sdg> Project_Sdg { get; set; }
        public virtual DbSet<Volunteer_Sdg> Volunteer_Sdg { get; set; }
        public virtual DbSet<Qrcode> Qrcode { get; set; }
        public virtual DbSet<Attendance_Entry> Attendance_Entry { get; set; }
        public virtual DbSet<Attendance_Register> Attendance_Register { get; set; }
        public virtual DbSet<Document> Document { get; set; }
      
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<Project_Register> Project_Register { get; set; }
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<NGO_Sdg> NGO_Sdg { get; set; }
        public virtual DbSet<Suspension> Suspension { get; set; }
        public virtual DbSet<Project_Summary> Project_Summary { get; set; }
        public virtual DbSet<Unregistered_Volunteer> Unregistered_Volunteer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Only the composite key needs to be here
            modelBuilder.Entity<Volunteer_Sdg>()
                .HasKey(vs => new { vs.Volunteer_Id, vs.Sdg_Id });

            modelBuilder.Entity<Volunteer_Sdg>()
                .HasOne(vs => vs.Volunteer)
                .WithMany(v => v.VolunteerSdgs)
                .HasForeignKey(vs => vs.Volunteer_Id);

            modelBuilder.Entity<Volunteer_Sdg>()
                .HasOne(vs => vs.Sdg)
                .WithMany(s => s.VolunteerSdgs)
                .HasForeignKey(vs => vs.Sdg_Id);

            modelBuilder.Entity<Project_Register>()
                .HasKey(pv => new { pv.Project_Id, pv.User_Id });
           
            modelBuilder.Entity<Project_Sdg>()
                 .HasKey(ps => new { ps.Project_Id, ps.Sdg_Id });

            modelBuilder.Entity<NGO_Sdg>()
                .HasKey(ns => new { ns.Ngo_Id, ns.Sdg_Id });

            modelBuilder.Entity<NGO_Sdg>()
                .HasOne(vs => vs.NGO_Application)
                .WithMany(v => v.NgoSdgs)
                .HasForeignKey(vs => vs.Ngo_Id);

            modelBuilder.Entity<NGO_Sdg>()
                .HasOne(vs => vs.Sdg)
                .WithMany(s => s.NgoSdgs)
                .HasForeignKey(vs => vs.Sdg_Id);

            modelBuilder.Entity<Project_Register>()
               .HasOne(pr => pr.Project)
               .WithMany(p => p.ProjectRegisters)
               .HasForeignKey(pr => pr.Project_Id);

            modelBuilder.Entity<Project_Register>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.ProjectRegisters)
                .HasForeignKey(pr => pr.User_Id);
           

            modelBuilder.Entity<Attendance_Register>()
                .HasOne(a => a.Project)
                .WithMany(p => p.AttendanceRegisters)
                .HasForeignKey(a => a.Project_Id);

            modelBuilder.Entity<Attendance_Entry>()
                .HasOne(a => a.Attendance_Register)
                .WithMany(r => r.AttendanceEntries)
                .HasForeignKey(a => a.Attendance_Id);

            modelBuilder.Entity<Attendance_Entry>()
                .HasOne(a => a.User)
                .WithMany(u => u.AttendanceEntries)
                .HasForeignKey(a => a.User_Id);

            // Preventing duplicate attendance
            modelBuilder.Entity<Attendance_Entry>()
                .HasIndex(a => new
                {
                    a.Attendance_Id,
                    a.User_Id
                })
                .IsUnique();

            modelBuilder.Entity<Attendance_Entry>()
                .HasIndex(x => new
                {
                    x.Attendance_Id,
                    x.User_Id
                })
                .IsUnique();
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Project_Summary)
                .WithMany(s => s.Photos)
                .HasForeignKey(p => p.Summary_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => new { n.User_Id, n.Created_At });
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasColumnType("varchar(20)");

            modelBuilder.Entity<Campus>()
                .Property(c => c.Code)
                .HasConversion<string>();

            modelBuilder.Entity<NGO_Application>()
                .Property(s => s.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Volunteer>()
                .Property(v => v.Volunteer_Type)
                .HasConversion<string>();

            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Project>()
                .Property(p => p.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Project>()
                .Property(p => p.Location_Scope)
                .HasConversion<string>();

            modelBuilder.Entity<Project>()
                .Property(p => p.Partner)
                .HasConversion<string>();

            modelBuilder.Entity<Notification>()
                .Property(n => n.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Year_Of_Study)
                .HasConversion<string>();
        }
    }
}
