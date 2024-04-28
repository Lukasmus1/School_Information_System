﻿using Microsoft.EntityFrameworkCore;
using VUTIS2.Common.Enums;
using VUTIS2.DAL.Entities;

namespace VUTIS2.DAL.Seeds;

public static class EnrollmentSeeds
{
    public static readonly EnrollmentEntity EmptyEnrollment = new()
    {
        Id = default,
        SubjectId = default,
        StudentId = default,
    };

    public static readonly EnrollmentEntity SampleEnrollment1 = new()
    {
        Id = Guid.NewGuid(),
        SubjectId = SubjectSeeds.SampleSubject1.Id,
        StudentId = StudentSeeds.SampleStudent1.Id,
    };

    public static readonly EnrollmentEntity SampleEnrollment2 = new()
    {
        Id = Guid.NewGuid(),
        SubjectId = SubjectSeeds.SampleSubject2.Id,
        StudentId = StudentSeeds.SampleStudent2.Id,
    };
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnrollmentEntity>().HasData(
            SampleEnrollment1,
            SampleEnrollment2
        );
    }
}
