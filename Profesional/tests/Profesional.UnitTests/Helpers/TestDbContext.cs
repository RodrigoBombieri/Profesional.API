using Microsoft.EntityFrameworkCore;
using Profesional.Application.Interfaces;
using Profesional.Domain.Entities;

namespace Profesional.UnitTests.Helpers;

/// <summary>DbContext en memoria que implementa IApplicationDbContext, aislado por test.</summary>
public class TestDbContext : DbContext, IApplicationDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public static TestDbContext Create()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TestDbContext(options);
    }
}
