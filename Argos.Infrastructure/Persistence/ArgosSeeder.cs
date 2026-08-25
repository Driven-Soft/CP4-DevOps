using Argos.Domain.Entities;
using Argos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Argos.Infrastructure.Persistence;

/// <summary>
/// Seeds do Argos: reproduz os dados mockados originais do app (ver
/// <c>SEED_DADOS_MOCK.md</c>) já mapeados para o schema da API — tipos de
/// ocorrência, usuários (Defesa Civil + um cidadão), zonas de risco, ocorrências,
/// comentários e alertas. Idempotente: cada item é checado por uma chave natural
/// (chave/email/nome/título), então pode rodar em todo start sem duplicar nada.
///
/// As FKs são resolvidas por consulta (não por IDs fixos), e os horários do feed
/// são calculados relativos a <see cref="DateTime.UtcNow"/> para o feed nascer "fresco".
/// </summary>
public static class ArgosSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ArgosContext>();

        // Etapa 1 — entidades-raiz (sem FK entre si). Salvamos para obter os IDs.
        await SeedTiposOcorrenciaAsync(context);
        await SeedUsuariosAsync(context);
        await SeedZonasRiscoAsync(context);
        await context.SaveChangesAsync();

        var agora = DateTime.UtcNow;

        // Etapa 2 — ocorrências (dependem de tipo + usuário). Salvamos para obter os IDs.
        await SeedOcorrenciasAsync(context, agora);
        await context.SaveChangesAsync();

        // Etapa 3 — comentários (dependem de ocorrência + usuário) e alertas (zona + usuário).
        await SeedComentariosAsync(context, agora);
        await SeedAlertasAsync(context, agora);
        await context.SaveChangesAsync();
    }

    // Emails usados como chave natural dos usuários do seed.
    private const string EmailDefesaCivil = "defesacivil@argos.gov.br";
    private const string EmailCidadao = "joao@email.com";

    private static async Task SeedTiposOcorrenciaAsync(ArgosContext context)
    {
        var tipos = new (string Nome, string Chave)[]
        {
            ("Alagamento", "alagamento"),
            ("Deslizamento", "deslizamento"),
            ("Outro", "outro"),
        };

        var chavesExistentes = await context.TiposOcorrencia
            .Select(t => t.Chave)
            .ToListAsync();

        foreach (var (nome, chave) in tipos)
        {
            if (!chavesExistentes.Contains(chave))
                context.TiposOcorrencia.Add(new TipoOcorrencia(nome, chave));
        }
    }

    private static async Task SeedUsuariosAsync(ArgosContext context)
    {
        var emailsExistentes = await context.Usuarios
            .Select(u => u.Email)
            .ToListAsync();

        // A Defesa Civil é autora dos alertas e dos comentários oficiais.
        if (!emailsExistentes.Contains(EmailDefesaCivil))
        {
            context.Usuarios.Add(new Usuario(
                nome: "Defesa Civil",
                email: EmailDefesaCivil,
                senha: "defesa123",
                telefone: "1133334444",
                tipoUsuario: TipoUsuario.DEFESA_CIVIL));
        }

        // Cidadão de exemplo — autor das ocorrências e dos comentários de cidadão
        // (o mock exibia "Você"; no banco é um usuário real).
        if (!emailsExistentes.Contains(EmailCidadao))
        {
            context.Usuarios.Add(new Usuario(
                nome: "João Silva",
                email: EmailCidadao,
                senha: "123456",
                telefone: "11999998888",
                tipoUsuario: TipoUsuario.CIDADAO));
        }
    }

    private static async Task SeedZonasRiscoAsync(ArgosContext context)
    {
        // nome = "localizacao" e regiao = "zona" no app; reproduzem o texto dos mocks.
        var zonas = new (string Nome, string Regiao, double Lat, double Lng, NivelRisco Nivel)[]
        {
            ("Marginal Tietê, Casa Verde", "Norte",   -23.5089, -46.6388, NivelRisco.CRITICO),
            ("Jardim Ângela, Zona Sul",    "Sul",     -23.6821, -46.7635, NivelRisco.ALTO),
            ("Vila Prudente, Zona Leste",  "Leste",   -23.5849, -46.5805, NivelRisco.MEDIO),
            ("Pinheiros, Zona Oeste",      "Oeste",   -23.5629, -46.6544, NivelRisco.MEDIO),
            ("Sé, Centro",                 "Central", -23.5505, -46.6333, NivelRisco.BAIXO),
        };

        var nomesExistentes = await context.ZonasRisco
            .Select(z => z.Nome)
            .ToListAsync();

        foreach (var (nome, regiao, lat, lng, nivel) in zonas)
        {
            if (!nomesExistentes.Contains(nome))
            {
                context.ZonasRisco.Add(new ZonaRisco(
                    nome: nome,
                    cidade: "São Paulo",
                    estado: "SP",
                    latitude: lat,
                    longitude: lng,
                    regiao: regiao,
                    nivelRiscoAtual: nivel));
            }
        }
    }

    private static async Task SeedOcorrenciasAsync(ArgosContext context, DateTime agora)
    {
        var tipoIdPorChave = await context.TiposOcorrencia
            .ToDictionaryAsync(t => t.Chave, t => t.Id);
        var cidadaoId = await context.Usuarios
            .Where(u => u.Email == EmailCidadao)
            .Select(u => u.Id)
            .SingleAsync();

        // Todas autoradas pelo cidadão; status final definido aqui (o domínio nasce EM_ANALISE).
        var ocorrencias = new[]
        {
            new { Titulo = "Rua alagada na Marginal",  Descricao = "Água acima do meio-fio, trânsito interrompido.", Chave = "alagamento",   Nivel = NivelRisco.CRITICO, Status = StatusOcorrencia.EQUIPE_A_CAMINHO, Bairro = "Bela Vista",    Lat = -23.5089, Lng = -46.6388, Offset = -5 },
            new { Titulo = "Deslizamento de barreira", Descricao = "Queda parcial de talude próximo a residências.", Chave = "deslizamento", Nivel = NivelRisco.ALTO,    Status = StatusOcorrencia.EQUIPE_NO_LOCAL,  Bairro = "Vila Madalena", Lat = -23.5905, Lng = -46.6533, Offset = -42 },
            new { Titulo = "Acúmulo de água em via",   Descricao = "Bueiro entupido causando poça grande.",          Chave = "alagamento",   Nivel = NivelRisco.MEDIO,   Status = StatusOcorrencia.EM_ANALISE,       Bairro = "Sé",            Lat = -23.5475, Lng = -46.6361, Offset = -95 },
            new { Titulo = "Risco de queda de árvore", Descricao = "Árvore inclinada após ventania, sem bloqueio.",  Chave = "outro",        Nivel = NivelRisco.BAIXO,   Status = StatusOcorrencia.RESOLVIDA,        Bairro = "Pinheiros",     Lat = -23.5629, Lng = -46.6544, Offset = -310 },
        };

        var titulosExistentes = await context.Ocorrencias
            .Select(o => o.Titulo)
            .ToListAsync();

        foreach (var dado in ocorrencias)
        {
            if (titulosExistentes.Contains(dado.Titulo))
                continue;

            var ocorrencia = new Ocorrencia(
                titulo: dado.Titulo,
                descricao: dado.Descricao,
                tipoOcorrenciaId: tipoIdPorChave[dado.Chave],
                usuarioId: cidadaoId,
                latitude: dado.Lat,
                longitude: dado.Lng,
                nivelRisco: dado.Nivel,
                bairro: dado.Bairro);

            if (dado.Status != StatusOcorrencia.EM_ANALISE)
                ocorrencia.AlterarStatus(dado.Status);

            var entry = context.Ocorrencias.Add(ocorrencia);
            // DataCriacao tem setter privado; o EF a escreve via change tracker.
            entry.Property(o => o.DataCriacao).CurrentValue = agora.AddMinutes(dado.Offset);
        }
    }

    private static async Task SeedComentariosAsync(ArgosContext context, DateTime agora)
    {
        var ocorrenciaIdPorTitulo = await context.Ocorrencias
            .ToDictionaryAsync(o => o.Titulo, o => o.Id);
        var usuarioIdPorEmail = await context.Usuarios
            .ToDictionaryAsync(u => u.Email, u => u.Id);

        // (título da ocorrência, email do autor, mensagem, offset em minutos)
        var comentarios = new[]
        {
            new { OcorrenciaTitulo = "Rua alagada na Marginal",  Email = EmailDefesaCivil, Mensagem = "Equipe a caminho do local. Evitem a via.",       Offset = -3 },
            new { OcorrenciaTitulo = "Rua alagada na Marginal",  Email = EmailCidadao,     Mensagem = "Obrigado, o nível da água continua subindo.",      Offset = -1 },
            new { OcorrenciaTitulo = "Deslizamento de barreira", Email = EmailDefesaCivil, Mensagem = "Área isolada, equipe no local avaliando o talude.", Offset = -20 },
        };

        // Chave natural composta para idempotência: (OcorrenciaId, Mensagem).
        var existentes = await context.ComentariosOcorrencia
            .Select(c => new { c.OcorrenciaId, c.Mensagem })
            .ToListAsync();

        foreach (var dado in comentarios)
        {
            var ocorrenciaId = ocorrenciaIdPorTitulo[dado.OcorrenciaTitulo];
            if (existentes.Any(e => e.OcorrenciaId == ocorrenciaId && e.Mensagem == dado.Mensagem))
                continue;

            var comentario = new ComentarioOcorrencia(
                mensagem: dado.Mensagem,
                ocorrenciaId: ocorrenciaId,
                usuarioId: usuarioIdPorEmail[dado.Email]);

            var entry = context.ComentariosOcorrencia.Add(comentario);
            entry.Property(c => c.DataCriacao).CurrentValue = agora.AddMinutes(dado.Offset);
        }
    }

    private static async Task SeedAlertasAsync(ArgosContext context, DateTime agora)
    {
        var zonaIdPorNome = await context.ZonasRisco
            .ToDictionaryAsync(z => z.Nome, z => z.Id);
        var defesaCivilId = await context.Usuarios
            .Where(u => u.Email == EmailDefesaCivil)
            .Select(u => u.Id)
            .SingleAsync();

        // Todos autorados pela Defesa Civil. InicioVigencia = criação; o último está
        // inativo e com vigência já expirada (FimOffset negativo).
        var alertas = new[]
        {
            new { Titulo = "Transbordamento do Rio Tietê",     Descricao = "Nível do rio acima da cota de inundação. Evacuação preventiva das áreas ribeirinhas em andamento.", Nivel = NivelRisco.CRITICO, ZonaNome = "Marginal Tietê, Casa Verde", Ativo = true,  CriacaoOffset = -8,   FimOffset = 180 },
            new { Titulo = "Risco de deslizamento em encosta", Descricao = "Solo saturado após chuvas intensas. Moradores de áreas de encosta devem redobrar a atenção.",        Nivel = NivelRisco.ALTO,    ZonaNome = "Jardim Ângela, Zona Sul",    Ativo = true,  CriacaoOffset = -35,  FimOffset = 240 },
            new { Titulo = "Alagamento de vias",               Descricao = "Pontos de acúmulo de água em vias principais. Trânsito lento e desvios pontuais.",                   Nivel = NivelRisco.MEDIO,   ZonaNome = "Vila Prudente, Zona Leste",  Ativo = true,  CriacaoOffset = -72,  FimOffset = 120 },
            new { Titulo = "Chuva forte com rajadas de vento", Descricao = "Previsão de temporal isolado nas próximas horas. Risco de queda de galhos e destelhamentos.",        Nivel = NivelRisco.MEDIO,   ZonaNome = "Pinheiros, Zona Oeste",      Ativo = true,  CriacaoOffset = -120, FimOffset = 90 },
            new { Titulo = "Atenção a córregos urbanos",       Descricao = "Situação controlada. Monitoramento preventivo dos níveis dos córregos da região central.",            Nivel = NivelRisco.BAIXO,   ZonaNome = "Sé, Centro",                 Ativo = false, CriacaoOffset = -260, FimOffset = -30 },
        };

        var titulosExistentes = await context.Alertas
            .Select(a => a.Titulo)
            .ToListAsync();

        foreach (var dado in alertas)
        {
            if (titulosExistentes.Contains(dado.Titulo))
                continue;

            var inicio = agora.AddMinutes(dado.CriacaoOffset);
            var alerta = new Alerta(
                titulo: dado.Titulo,
                descricao: dado.Descricao,
                nivelAlerta: dado.Nivel,
                zonaRiscoId: zonaIdPorNome[dado.ZonaNome],
                usuarioCriadorId: defesaCivilId,
                inicioVigencia: inicio,
                fimVigencia: agora.AddMinutes(dado.FimOffset));

            if (!dado.Ativo)
                alerta.Desativar();

            var entry = context.Alertas.Add(alerta);
            entry.Property(a => a.DataCriacao).CurrentValue = inicio;
        }
    }
}
