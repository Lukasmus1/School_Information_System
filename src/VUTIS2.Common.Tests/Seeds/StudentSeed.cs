using Microsoft.EntityFrameworkCore;
using VUTIS2.DAL.Entities;

namespace VUTIS2.Common.Tests.Seeds;
    public static class StudentSeeds
    {
        public static readonly StudentEntity EmptyStudent = new()
        {
            Id = default, FirstName = default!, LastName = default!, PhotoUrl = default,
        };

        public static readonly StudentEntity SampleStudent1 = new()
        {
            Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhotoUrl = null,
        };

        public static readonly StudentEntity SampleStudent2 = new()
        {
            Id = Guid.NewGuid(), FirstName = "Jacob", LastName = "Done", PhotoUrl = null,
        };

        static StudentSeeds()
        {
            SampleStudent1.Enrollments?.Add(EnrollmentSeeds.SampleEnrollment1);
            SampleStudent2.Enrollments?.Add(EnrollmentSeeds.SampleEnrollment2);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasData(
                SampleStudent1 with {Enrollments = Array.Empty<EnrollmentEntity>()},
                SampleStudent2 with {Enrollments = Array.Empty<EnrollmentEntity>()}
            );
        }
    }
