// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//


using System.Text;
using System.Reflection.Emit;
using VUTIS2.Common.Tests.Seeds;
using VUTIS2.DAL;
using Microsoft.EntityFrameworkCore;

namespace VUTIS2.Common.Tests;

public class SchoolTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
    : SchoolDbContext(contextOptions, seedDemoData: false)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (seedTestingData)
        {
            SubjectSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
            StudentSeeds.Seed(modelBuilder);
            EvaluationSeeds.Seed(modelBuilder);
            EnrollmentSeeds.Seed(modelBuilder);
        }
    }
}
