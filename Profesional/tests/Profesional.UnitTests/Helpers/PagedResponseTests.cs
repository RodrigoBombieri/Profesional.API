using FluentAssertions;
using Profesional.Application.Helpers;

namespace Profesional.UnitTests.Helpers;

public class PagedResponseTests
{
    [Fact]
    public void PageSize_NoSuperaElMaximoDe50()
    {
        var p = new PaginationParams { PageSize = 500 };
        p.PageSize.Should().Be(50);
    }

    [Fact]
    public void PageSize_RespetaValoresDentroDelLimite()
    {
        var p = new PaginationParams { PageSize = 20 };
        p.PageSize.Should().Be(20);
    }

    [Theory]
    [InlineData(25, 10, 3)]
    [InlineData(20, 10, 2)]
    [InlineData(0, 10, 0)]
    public void TotalPages_SeRedondeaHaciaArriba(int total, int size, int esperado)
    {
        var r = new PagedResponse<int>(new List<int>(), 1, size, total);
        r.TotalPages.Should().Be(esperado);
    }

    [Fact]
    public void HasNextPage_HasPreviousPage_SegunLaPagina()
    {
        var primera = new PagedResponse<int>(new List<int>(), 1, 10, 25);
        var ultima = new PagedResponse<int>(new List<int>(), 3, 10, 25);

        primera.HasPreviousPage.Should().BeFalse();
        primera.HasNextPage.Should().BeTrue();
        ultima.HasPreviousPage.Should().BeTrue();
        ultima.HasNextPage.Should().BeFalse();
    }
}
