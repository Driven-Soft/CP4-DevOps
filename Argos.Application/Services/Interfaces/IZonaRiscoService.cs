using Argos.Application.DTOs;

namespace Argos.Application.Services.Interfaces;

public interface IZonaRiscoService
{
    ZonaRiscoResponse Create(ZonaRiscoRequest request);
    IReadOnlyCollection<ZonaRiscoResponse> GetAll();
    ZonaRiscoResponse? GetById(int id);
    ZonaRiscoResponse? Update(int id, ZonaRiscoPatchRequest request);
    bool Delete(int id);
}
