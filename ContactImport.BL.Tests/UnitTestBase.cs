using System;
using ContactImport.DAL;
using Microsoft.EntityFrameworkCore;

namespace ContactImport.BL.Tests;

public abstract class UnitTestBase : IDisposable
{
    public UnitTestBase()
    {
        DbContext = new AppDbContext(new DbContextOptionsBuilder().UseInMemoryDatabase("TESTDB").UseLazyLoadingProxies().Options);
    }

    protected AppDbContext DbContext;

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}