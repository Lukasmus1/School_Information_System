﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.EntityFrameworkCore;
using VUTIS2.DAL.Entities;

namespace VUTIS2.Common.Tests.Seeds
{
    public static class EvaluationSeeds
    {
        public static readonly EvaluationEntity EmptyEvaluation = new()
        {
            Id = default,
            Points = default,
            Description = default!,
            StudentId = default,
            ActivityId = default,
            Activity = default,
            Student = default
        };

        public static readonly EvaluationEntity SampleEvaluation1 = new()
        {
            Id = Guid.NewGuid(),
            Points = 90,
            Description = "First question is wrong, ...",
            StudentId = StudentSeeds.SampleStudent1.Id,
            Student = StudentSeeds.SampleStudent1,
            ActivityId = ActivitySeeds.SampleActivity1.Id,
            Activity = ActivitySeeds.SampleActivity1,
        };

        public static readonly EvaluationEntity SampleEvaluation2 = new()
        {
            Id = Guid.NewGuid(),
            Points = 45,
            Description = "Did not pass...",
            StudentId = StudentSeeds.SampleStudent2.Id,
            Student = StudentSeeds.SampleStudent2,
            ActivityId = ActivitySeeds.SampleActivity2.Id,
            Activity = ActivitySeeds.SampleActivity2,
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EvaluationEntity>().HasData(
                SampleEvaluation2 with { Activity = null!, Student = null! },
                SampleEvaluation1 with { Activity = null!, Student = null! }
            );
        }
    }
}

