using FluentAssertions;
using Profesional.Application.DTOs;
using Profesional.Application.Helpers;
using Profesional.Application.Services;
using Profesional.Domain.Entities;
using Profesional.UnitTests.Helpers;

namespace Profesional.UnitTests.Services;

public class PacienteServiceTests
{
    private static PacienteCreateDto Dto(string nombre = "Ana", string apellido = "Pérez",
        string dni = "30111222", string email = "ana@test.com") => new()
    {
        Nombre = nombre, Apellido = apellido, DNI = dni, Email = email,
        Telefono = "3442000000", Direccion = "Calle 1", ObraSocial = "OSDE"
    };

    [Fact]
    public async Task CreateAsync_PersisteElPacienteComoActivo()
    {
        using var ctx = TestDbContext.Create();
        var sut = new PacienteService(ctx);

        var result = await sut.CreateAsync(Dto());

        result.Id.Should().BeGreaterThan(0);
        result.Activo.Should().BeTrue();
        ctx.Pacientes.Should().ContainSingle(p => p.DNI == "30111222");
    }

    [Fact]
    public async Task GetAllAsync_NoIncluyePacientesInactivos_PorDefecto()
    {
        using var ctx = TestDbContext.Create();
        ctx.Pacientes.AddRange(
            new Paciente { Nombre = "Activo", Apellido = "Uno", DNI = "1", Email = "a@a.com", Activo = true },
            new Paciente { Nombre = "Baja", Apellido = "Dos", DNI = "2", Email = "b@b.com", Activo = false });
        await ctx.SaveChangesAsync();
        var sut = new PacienteService(ctx);

        var result = await sut.GetAllAsync(new PaginationParams());

        result.TotalRecords.Should().Be(1);
        result.Data.Should().OnlyContain(p => p.Nombre == "Activo");
    }

    [Fact]
    public async Task GetAllAsync_BuscaPorNombreSinImportarMayusculas()
    {
        using var ctx = TestDbContext.Create();
        ctx.Pacientes.AddRange(
            new Paciente { Nombre = "Lucía", Apellido = "Gómez", DNI = "1", Email = "l@l.com" },
            new Paciente { Nombre = "Marcos", Apellido = "Díaz", DNI = "2", Email = "m@m.com" });
        await ctx.SaveChangesAsync();
        var sut = new PacienteService(ctx);

        var result = await sut.GetAllAsync(new PaginationParams { Search = "LUC" });

        result.Data.Should().ContainSingle(p => p.Nombre == "Lucía");
    }

    [Fact]
    public async Task GetAllAsync_PaginaYOrdenaDescendentePorApellido()
    {
        using var ctx = TestDbContext.Create();
        for (var i = 1; i <= 5; i++)
            ctx.Pacientes.Add(new Paciente { Nombre = "N" + i, Apellido = "Ap" + i, DNI = i.ToString(), Email = $"{i}@t.com" });
        await ctx.SaveChangesAsync();
        var sut = new PacienteService(ctx);

        var result = await sut.GetAllAsync(new PaginationParams
        {
            PageNumber = 1, PageSize = 2, SortBy = "apellido", SortDescending = true
        });

        result.TotalRecords.Should().Be(5);
        result.TotalPages.Should().Be(3);
        result.Data.Select(p => p.Apellido).Should().Equal("Ap5", "Ap4");
    }

    [Fact]
    public async Task GetByIdAsync_DevuelveNull_SiElPacienteEstaInactivo()
    {
        using var ctx = TestDbContext.Create();
        var p = new Paciente { Nombre = "X", Apellido = "Y", DNI = "9", Email = "x@y.com", Activo = false };
        ctx.Pacientes.Add(p);
        await ctx.SaveChangesAsync();
        var sut = new PacienteService(ctx);

        (await sut.GetByIdAsync(p.Id)).Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_IncluyeSesionesOrdenadasPorFechaDescendente()
    {
        using var ctx = TestDbContext.Create();
        var p = new Paciente { Nombre = "Ana", Apellido = "Paz", DNI = "5", Email = "a@p.com" };
        p.Sesiones.Add(new Sesion { Fecha = new DateTime(2026, 1, 1), TipoTratamiento = "Vieja" });
        p.Sesiones.Add(new Sesion { Fecha = new DateTime(2026, 6, 1), TipoTratamiento = "Nueva" });
        ctx.Pacientes.Add(p);
        await ctx.SaveChangesAsync();
        var sut = new PacienteService(ctx);

        var result = await sut.GetByIdAsync(p.Id);

        result!.Sesiones.Select(s => s.TipoTratamiento).Should().Equal("Nueva", "Vieja");
    }

    [Fact]
    public async Task UpdateAsync_ModificaLosDatos()
    {
        using var ctx = TestDbContext.Create();
        var sut = new PacienteService(ctx);
        var creado = await sut.CreateAsync(Dto());

        var result = await sut.UpdateAsync(creado.Id, Dto(nombre: "Ana María"));

        result!.Nombre.Should().Be("Ana María");
    }

    [Fact]
    public async Task UpdateAsync_DevuelveNull_SiNoExiste()
    {
        using var ctx = TestDbContext.Create();
        var sut = new PacienteService(ctx);

        (await sut.UpdateAsync(999, Dto())).Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_HaceBajaLogica_SinBorrarElRegistro()
    {
        using var ctx = TestDbContext.Create();
        var sut = new PacienteService(ctx);
        var creado = await sut.CreateAsync(Dto());

        var ok = await sut.DeleteAsync(creado.Id);

        ok.Should().BeTrue();
        ctx.Pacientes.Single().Activo.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_DevuelveFalse_SiNoExiste()
    {
        using var ctx = TestDbContext.Create();
        var sut = new PacienteService(ctx);

        (await sut.DeleteAsync(123)).Should().BeFalse();
    }
}
