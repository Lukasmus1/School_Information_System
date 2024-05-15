using Microsoft.EntityFrameworkCore;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Seeds;

namespace VUTIS2.DAL
{
    public class SchoolDbContext(DbContextOptions contextOptions, bool seedDemoData) : DbContext(contextOptions)
    {
        public DbSet<StudentEntity> Students => Set<StudentEntity>();
        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        public DbSet<SubjectEntity> Subjects => Set<SubjectEntity>();
        public DbSet<EvaluationEntity> Evaluations => Set<EvaluationEntity>();
        public DbSet<EnrollmentEntity> Enrollments => Set<EnrollmentEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Removed redundant relations (EF core infers them automatically from one side of the relation)
            modelBuilder.Entity<StudentEntity>()
                .HasMany(i => i.Enrollments).WithOne(i=>i.Student);
            modelBuilder.Entity<SubjectEntity>()
                .HasMany(i => i.Activities)
                .WithOne(i => i.Subject);
            modelBuilder.Entity<SubjectEntity>()
                .HasMany(i => i.Enrollments).WithOne(i=>i.Subject);
            //modelBuilder.Entity<EnrollmentEntity>();
            modelBuilder.Entity<ActivityEntity>()
                .HasMany(i => i.Evaluations)
                .WithOne(i => i.Activity);

            modelBuilder.Entity<EvaluationEntity>()
                .HasOne(i => i.Student);


            if (seedDemoData)
            {
                StudentSeeds.Seed(modelBuilder);
                SubjectSeeds.Seed(modelBuilder);
                ActivitySeeds.Seed(modelBuilder);
                EvaluationSeeds.Seed(modelBuilder);
                EnrollmentSeeds.Seed(modelBuilder);

            }
        }
    }
}
