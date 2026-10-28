using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Profesional.Application.DTOs;
using Profesional.Application.Services;
using Profesional.UnitTests.Helpers;

namespace Profesional.UnitTests.Services;

public class AuthServiceTests
{
    // Sin claves configuradas: el servicio usa sus valores por defecto de desarrollo
    private static AuthService CreateSut(TestDbContext ctx) =>
        new(ctx, new Mock<IConfiguration>().Object);

    private static UsuarioRegisterDto Registro(string email = "kine@test.com") =>
        new() { Email = email, Password = "secreto123", Nombre = "Kine" };

    [Fact]
    public async Task RegisterAsync_HasheaLaContraseña_YDevuelveToken()
    {
        using var ctx = TestDbContext.Create();
        var sut = CreateSut(ctx);

        var result = await sut.RegisterAsync(Registro());

        result.Token.Should().NotBeNullOrWhiteSpace();
        result.Rol.Should().Be("Kinesiologo");
        var guardado = ctx.Usuarios.Single();
        guardado.PasswordHash.Should().NotBe("secreto123");
        BCrypt.Net.BCrypt.Verify("secreto123", guardado.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_Falla_SiElEmailYaExiste()
    {
        using var ctx = TestDbContext.Create();
        var sut = CreateSut(ctx);
        await sut.RegisterAsync(Registro());

        var act = () => sut.RegisterAsync(Registro());

        await act.Should().ThrowAsync<Exception>().WithMessage("*ya está registrado*");
    }

    [Fact]
    public async Task LoginAsync_ConCredencialesCorrectas_DevuelveToken()
    {
        using var ctx = TestDbContext.Create();
        var sut = CreateSut(ctx);
        await sut.RegisterAsync(Registro());

        var result = await sut.LoginAsync(new UsuarioLoginDto { Email = "kine@test.com", Password = "secreto123" });

        result.Email.Should().Be("kine@test.com");
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ConContraseñaIncorrecta_Falla()
    {
        using var ctx = TestDbContext.Create();
        var sut = CreateSut(ctx);
        await sut.RegisterAsync(Registro());

        var act = () => sut.LoginAsync(new UsuarioLoginDto { Email = "kine@test.com", Password = "otra" });

        await act.Should().ThrowAsync<Exception>().WithMessage("*incorrectos*");
    }

    [Fact]
    public async Task LoginAsync_ConEmailInexistente_FallaConElMismoMensaje()
    {
        using var ctx = TestDbContext.Create();
        var sut = CreateSut(ctx);

        var act = () => sut.LoginAsync(new UsuarioLoginDto { Email = "nadie@test.com", Password = "x" });

        await act.Should().ThrowAsync<Exception>().WithMessage("*incorrectos*");
    }
}
