﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using VUTIS2.Common.Tests.Seeds;
using VUTIS2.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VUTIS2.DAL.Tests;
using Xunit;
using Xunit.Abstractions;
using VUTIS2.Common.Enums;
using VUTIS2.Common.Tests;

namespace VUTIS2.DAL.Tests;
public class DbContextActivityTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Activity_Persisted()
    {
        // Arrange
        var entity = ActivitySeeds.EmptyActivity with
        {
            Id = Guid.NewGuid(),
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(4),
            RoomName = "A100",
            ActivityType = ActivityType.Exam,
            Description = "Test activity description",
            SubjectId = SubjectSeeds.SampleSubject.Id,
            Subject = SubjectSeeds.SampleSubject,
            Evaluation = new EvaluationEntity[]
            {
                EvaluationSeeds.SampleEvaluation1,
                EvaluationSeeds.SampleEvaluation2
            }
        };

        // Act
        SchoolDbContextSUT.Activities.Add(entity);
        await SchoolDbContextSUT.SaveChangesAsync();

        // Assert
        var actualEntity = await SchoolDbContextSUT.Activities
            .Include(a => a.Evaluation)
            .SingleOrDefaultAsync(a => a.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetAll_Activities_ContainsSeededActivities()
    {
        // Act
        var entities = await SchoolDbContextSUT.Activities.ToListAsync();

        // Assert
        DeepAssert.Contains(ActivitySeeds.SampleActivity1, entities);
        DeepAssert.Contains(ActivitySeeds.SampleActivity2, entities);
    }

    [Fact]
    public async Task GetById_Activity()
    {
        // Arrange
        var entityId = ActivitySeeds.SampleActivity1.Id;

        // Act
        var entity = await SchoolDbContextSUT.Activities.FindAsync(entityId);

        // Assert
        DeepAssert.Equal(ActivitySeeds.SampleActivity1, entity);
    }

    [Fact]
    public async Task GetById_IncludingEvaluation_Activity()
    {
        // Arrange
        var entityId = ActivitySeeds.SampleActivity1.Id;

        // Act
        var entity = await SchoolDbContextSUT.Activities
            .Include(a => a.Evaluation)
            .SingleOrDefaultAsync(a => a.Id == entityId);

        // Assert
        DeepAssert.Equal(ActivitySeeds.SampleActivity1, entity);
    }

    [Fact]
    public async Task Update_Activity_Persisted()
    {
        // Arrange
        var entity = ActivitySeeds.SampleActivity1;
        entity.RoomName = "Updated Room";

        // Act
        SchoolDbContextSUT.Update(entity);
        await SchoolDbContextSUT.SaveChangesAsync();

        // Assert
        var updatedEntity = await SchoolDbContextSUT.Activities.FindAsync(entity.Id);
        DeepAssert.Equal("Updated Room", updatedEntity.RoomName);
    }

    [Fact]
    public async Task Delete_Activity_Deleted()
    {
        // Arrange
        var entity = ActivitySeeds.SampleActivity1;

        // Act
        SchoolDbContextSUT.Activities.Remove(entity);
        await SchoolDbContextSUT.SaveChangesAsync();

        // Assert
        var deletedEntity = await SchoolDbContextSUT.Activities.FindAsync(entity.Id);
        Assert.Null(deletedEntity);
    }
}

