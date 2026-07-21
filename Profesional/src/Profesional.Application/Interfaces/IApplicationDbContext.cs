using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Profesional.Domain.Entities;

namespace Profesional.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Paciente> Pacientes { get; }
        DbSet<Sesion> Sesiones { get; }
        DbSet<Usuario> Usuarios { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
