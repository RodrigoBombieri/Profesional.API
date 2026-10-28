using FluentAssertions;
using Profesional.Application.DTOs;
using Profesional.Application.Services;
using Profesional.Domain.Entities;
using Profesional.UnitTests.Helpers;

namespace Profesional.UnitTests.Services;

public class SesionServiceTests
{
    private static async Task<Paciente> SeedPaciente(TestDbContext ctx, bool activo = true)
    {
        var p = new Paciente { Nombre = "Ana", Apellido = "Paz", DNI = Guid.NewGuid().ToString(), Email = Guid.NewGuid() + "@t.com", Activo = activo };
        ctx.Pacientes.Add(p);
        await ctx.SaveChangesAsync();
        return p;
    }

    private static SesionCreateDto Dto(int pacienteId) => new()
    {
        PacienteId = pacienteId,
        Fecha = new DateTime(2026, 5, 10, 9, 0, 0),
        TipoTratamiento = "Rehabilitación",
        DuracionMinutos = 45
    };

    [Fact]
    public async Task CreateAsync_DevuelveElNombreDelPaciente()
    {
        using var ctx = TestDbContext.Create();
        var p = await SeedPaciente(ctx);
        var sut = new SesionService(ctx);

        var result = await sut.CreateAsync(Dto(p.Id));

        result.PacienteNombre.Should().Be("Ana Paz");
        result.Completada.Should().BeFalse();
        ctx.Sesiones.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateAsync_Falla_SiElPacienteEstaInactivo()
    {
        using var ctx = TestDbContext.Create();
        var p = await SeedPaciente(ctx, activo: false);
        var sut = new SesionService(ctx);

        var act = () => sut.CreateAsync(Dto(p.Id));

        await act.Should().ThrowAsync<Exception>().WithMessage("*no existe o está inactivo*");
        ctx.Sesiones.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ExcluyeSesionesDePacientesInactivos_YOrdenaPorFechaDesc()
    {
        using var ctx = TestDbContext.Create();
        var activo = await SeedPaciente(ctx);
        var baja = await SeedPaciente(ctx, activo: false);
        ctx.Sesiones.AddRange(
            new Sesion { PacienteId = activo.Id, Fecha = new DateTime(2026, 1, 1), TipoTratamiento = "Vieja" },
            new Sesion { PacienteId = activo.Id, Fecha = new DateTime(2026, 3, 1), TipoTratamiento = "Nueva" },
            new Sesion { PacienteId = baja.Id, Fecha = new DateTime(2026, 4, 1), TipoTratamiento = "DeBaja" });
        await ctx.SaveChangesAsync();
        var sut = new SesionService(ctx);

        var result = (await sut.GetAllAsync()).ToList();

        result.Select(s => s.TipoTratamiento).Should().Equal("Nueva", "Vieja");
    }

    [Fact]
    public async Task CompletarSesionAsync_MarcaLaSesionComoCompletada()
    {
        using var ctx = TestDbContext.Create();
        var p = await SeedPaciente(ctx);
        var sut = new SesionService(ctx);
        var creada = await sut.CreateAsync(Dto(p.Id));

        var ok = await sut.CompletarSesionAsync(creada.Id);

        ok.Should().BeTrue();
        ctx.Sesiones.Single().Completada.Should().BeTrue();
    }

    [Fact]
    public async Task CompletarSesionAsync_DevuelveFalse_SiNoExiste()
    {
        using var ctx = TestDbContext.Create();
        var sut = new SesionService(ctx);

        (await sut.CompletarSesionAsync(404)).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_BorraFisicamenteLaSesion()
    {
        using var ctx = TestDbContext.Create();
        var p = await SeedPaciente(ctx);
        var sut = new SesionService(ctx);
        var creada = await sut.CreateAsync(Dto(p.Id));

        (await sut.DeleteAsync(creada.Id)).Should().BeTrue();
        ctx.Sesiones.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_DevuelveNull_SiLaSesionNoExiste()
    {
        using var ctx = TestDbContext.Create();
        var p = await SeedPaciente(ctx);
        var sut = new SesionService(ctx);

        (await sut.UpdateAsync(777, Dto(p.Id))).Should().BeNull();
    }
}
