using System;
using ContactImport.DAL;
using Microsoft.EntityFrameworkCore;

namespace ContactImport.BL.Tests;

public abstract class UnitTestBase : IDisposable
{
    protected UnitTestBase()
    {
        DbContext = new AppDbContext(new DbContextOptionsBuilder().UseInMemoryDatabase("TESTDB").UseLazyLoadingProxies().Options);
    }

    protected readonly AppDbContext DbContext;

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}