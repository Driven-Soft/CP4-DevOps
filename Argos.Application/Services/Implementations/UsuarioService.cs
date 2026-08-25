using Argos.Application.DTOs;
using Argos.Application.Interfaces.Repositories;
using Argos.Application.Services.Interfaces;

namespace Argos.Application.Services.Implementations;

public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
{
    public UsuarioResponse Create(UsuarioRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (repository.GetByEmail(email) is not null)
            throw new ArgumentException("Já existe um usuário com este email");

        var usuario = request.ToDomain();
        repository.Add(usuario);
        repository.SaveChanges();
        return new UsuarioResponse(usuario);
    }

    public IReadOnlyCollection<UsuarioResponse> GetAll() =>
        repository.GetAll().Select(u => new UsuarioResponse(u)).ToList();

    public UsuarioResponse? GetById(int id)
    {
        var usuario = repository.GetById(id);
        return usuario is null ? null : new UsuarioResponse(usuario);
    }

    public UsuarioResponse? Update(int id, UsuarioPatchRequest request)
    {
        var usuario = repository.GetById(id);
        if (usuario is null) return null;

        if (request.Nome is not null) usuario.UpdateNome(request.Nome);
        if (request.Senha is not null) usuario.UpdateSenha(request.Senha);
        if (request.Telefone is not null) usuario.UpdateTelefone(request.Telefone);
        if (request.TipoUsuario is not null) usuario.AlterarTipo(request.TipoUsuario.Value);
        if (request.Ativo is not null)
        {
            if (request.Ativo.Value) usuario.Ativar();
            else usuario.Desativar();
        }

        repository.Update(usuario);
        repository.SaveChanges();
        return new UsuarioResponse(usuario);
    }

    public bool Delete(int id)
    {
        var usuario = repository.GetById(id);
        if (usuario is null) return false;

        // Soft delete: usuário com ocorrências/comentários não é apagado fisicamente.
        usuario.Desativar();
        repository.Update(usuario);
        repository.SaveChanges();
        return true;
    }
}
