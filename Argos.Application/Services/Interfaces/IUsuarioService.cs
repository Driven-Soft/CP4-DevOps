using Argos.Application.DTOs;

namespace Argos.Application.Services.Interfaces;

public interface IUsuarioService
{
    UsuarioResponse Create(UsuarioRequest request);
    IReadOnlyCollection<UsuarioResponse> GetAll();
    UsuarioResponse? GetById(int id);
    UsuarioResponse? Update(int id, UsuarioPatchRequest request);
    /// <summary>Soft delete (<c>Desativar</c>) — usuário com ocorrências não é apagado.</summary>
    bool Delete(int id);
}
