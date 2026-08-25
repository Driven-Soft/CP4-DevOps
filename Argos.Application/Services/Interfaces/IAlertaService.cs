using Argos.Application.DTOs;
using Argos.Domain.Enums;

namespace Argos.Application.Services.Interfaces;

public interface IAlertaService
{
    AlertaResponse Create(AlertaRequest request);
    /// <summary>Feed filtrável (<c>?status=&amp;nivel=</c>).</summary>
    IReadOnlyCollection<AlertaResponse> Buscar(bool? apenasAtivos, NivelRisco? nivel);
    AlertaResponse? GetById(int id);
    AlertaResponse? Update(int id, AlertaPatchRequest request);
    bool Delete(int id);
}
