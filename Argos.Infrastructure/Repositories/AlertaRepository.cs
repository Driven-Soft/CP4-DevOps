using Argos.Application.Interfaces.Repositories;
using Argos.Domain.Entities;
using Argos.Domain.Enums;
using Argos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Repositories;

public class AlertaRepository(ArgosContext context)
    : Repository<Alerta>(context), IAlertaRepository
{
    public IReadOnlyCollection<Alerta> Buscar(bool? apenasAtivos, NivelRisco? nivel)
    {
        var query = Set.AsNoTracking()
            .Include(a => a.ZonaRisco)
            .Include(a => a.UsuarioCriador)
            .AsQueryable();

        if (apenasAtivos.HasValue)
            query = query.Where(a => a.Ativo == apenasAtivos.Value);
        if (nivel.HasValue)
            query = query.Where(a => a.NivelAlerta == nivel.Value);

        return query.OrderByDescending(a => a.DataCriacao).ToList();
    }

    public Alerta? GetByIdCompleto(int id) =>
        Set.AsNoTracking()
            .Include(a => a.ZonaRisco)
            .Include(a => a.UsuarioCriador)
            .FirstOrDefault(a => a.Id == id);
}
