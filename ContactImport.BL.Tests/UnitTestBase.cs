using System;
using AutoMapper;
using ContactImport.DAL;
using Microsoft.EntityFrameworkCore;

namespace ContactImport.BL.Tests;

public abstract class UnitTestBase : IDisposable
{
    protected UnitTestBase()
    {
        DbContext = new AppDbContext(new DbContextOptionsBuilder().UseInMemoryDatabase("TESTDB").UseLazyLoadingProxies()
            .Options);
        Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(typeof(AutoMapperConfig))));
    }

    protected Mapper Mapper { get; set; }

    protected readonly AppDbContext DbContext;

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}