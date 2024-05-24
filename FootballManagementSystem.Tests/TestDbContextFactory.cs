using System;
using Microsoft.EntityFrameworkCore;
using FootballManagementSystem.Models;

namespace FootballManagementSystem.Tests;

public class TestDbContextFactory
{
    public FootballContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FootballContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new FootballContext(options);
    }
}