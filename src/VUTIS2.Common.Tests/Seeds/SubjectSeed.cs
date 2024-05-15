using Microsoft.EntityFrameworkCore;
using VUTIS2.DAL.Entities;

namespace VUTIS2.Common.Tests.Seeds;

    public static class SubjectSeeds
    {
        public static readonly SubjectEntity EmptySubject = new()
        {
            Id = default,
            Name = default!,
            Abbreviation = default!,

        };

        public static readonly SubjectEntity SampleSubject1 = new()
        {
            Id = Guid.NewGuid(),
            Name = "Seminar C#",
            Abbreviation = "ICS",
        };

        public static readonly SubjectEntity SampleSubject2 = new()
        {
            Id = Guid.NewGuid(),
            Name = "Databaze",
            Abbreviation = "IDS",
        };

        static SubjectSeeds()
        {
            SampleSubject1.Activities?.Add(ActivitySeeds.SampleActivity1);
            SampleSubject2.Activities?.Add(ActivitySeeds.SampleActivity2);

            SampleSubject1.Enrollments?.Add(EnrollmentSeeds.SampleEnrollment1);
            SampleSubject2.Enrollments?.Add(EnrollmentSeeds.SampleEnrollment2);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(
                SampleSubject1 with { Enrollments = Array.Empty<EnrollmentEntity>(), Activities = Array.Empty<ActivityEntity>() },
                SampleSubject2 with { Enrollments = Array.Empty<EnrollmentEntity>(), Activities = Array.Empty<ActivityEntity>() }
            );
        }
    }
