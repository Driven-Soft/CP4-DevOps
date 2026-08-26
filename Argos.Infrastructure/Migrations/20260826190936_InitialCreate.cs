using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TIPOS_OCORRENCIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Chave = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPOS_OCORRENCIA", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoUsuario = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UltimoAcessoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ZONAS_RISCO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Regiao = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelRiscoAtual = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativa = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZONAS_RISCO", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ALERTAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelAlerta = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZonaRiscoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCriadorId = table.Column<int>(type: "int", nullable: true),
                    InicioVigencia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FimVigencia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ALERTAS_USUARIOS_UsuarioCriadorId",
                        column: x => x.UsuarioCriadorId,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ALERTAS_ZONAS_RISCO_ZonaRiscoId",
                        column: x => x.ZonaRiscoId,
                        principalTable: "ZONAS_RISCO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OCORRENCIAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoOcorrenciaId = table.Column<int>(type: "int", nullable: false),
                    NivelRisco = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ZonaRiscoId = table.Column<int>(type: "int", nullable: true),
                    Bairro = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResolvidoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OCORRENCIAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OCORRENCIAS_TIPOS_OCORRENCIA_TipoOcorrenciaId",
                        column: x => x.TipoOcorrenciaId,
                        principalTable: "TIPOS_OCORRENCIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCORRENCIAS_USUARIOS_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCORRENCIAS_ZONAS_RISCO_ZonaRiscoId",
                        column: x => x.ZonaRiscoId,
                        principalTable: "ZONAS_RISCO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LOGS_ALERTA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlertaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Acao = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DadosAntes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DadosDepois = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOGS_ALERTA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LOGS_ALERTA_ALERTAS_AlertaId",
                        column: x => x.AlertaId,
                        principalTable: "ALERTAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOGS_ALERTA_USUARIOS_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "COMENTARIOS_OCORRENCIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Mensagem = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OcorrenciaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMENTARIOS_OCORRENCIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COMENTARIOS_OCORRENCIA_OCORRENCIAS_OcorrenciaId",
                        column: x => x.OcorrenciaId,
                        principalTable: "OCORRENCIAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMENTARIOS_OCORRENCIA_USUARIOS_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ALERTAS_Ativo",
                table: "ALERTAS",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_ALERTAS_NivelAlerta",
                table: "ALERTAS",
                column: "NivelAlerta");

            migrationBuilder.CreateIndex(
                name: "IX_ALERTAS_UsuarioCriadorId",
                table: "ALERTAS",
                column: "UsuarioCriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ALERTAS_ZonaRiscoId",
                table: "ALERTAS",
                column: "ZonaRiscoId");

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIOS_OCORRENCIA_OcorrenciaId_DataCriacao",
                table: "COMENTARIOS_OCORRENCIA",
                columns: new[] { "OcorrenciaId", "DataCriacao" });

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIOS_OCORRENCIA_UsuarioId",
                table: "COMENTARIOS_OCORRENCIA",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_LOGS_ALERTA_AlertaId",
                table: "LOGS_ALERTA",
                column: "AlertaId");

            migrationBuilder.CreateIndex(
                name: "IX_LOGS_ALERTA_UsuarioId",
                table: "LOGS_ALERTA",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OCORRENCIAS_DataCriacao",
                table: "OCORRENCIAS",
                column: "DataCriacao",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_OCORRENCIAS_Status",
                table: "OCORRENCIAS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OCORRENCIAS_TipoOcorrenciaId",
                table: "OCORRENCIAS",
                column: "TipoOcorrenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_OCORRENCIAS_UsuarioId",
                table: "OCORRENCIAS",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OCORRENCIAS_ZonaRiscoId",
                table: "OCORRENCIAS",
                column: "ZonaRiscoId");

            migrationBuilder.CreateIndex(
                name: "IX_TIPOS_OCORRENCIA_Chave",
                table: "TIPOS_OCORRENCIA",
                column: "Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_Email",
                table: "USUARIOS",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMENTARIOS_OCORRENCIA");

            migrationBuilder.DropTable(
                name: "LOGS_ALERTA");

            migrationBuilder.DropTable(
                name: "OCORRENCIAS");

            migrationBuilder.DropTable(
                name: "ALERTAS");

            migrationBuilder.DropTable(
                name: "TIPOS_OCORRENCIA");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropTable(
                name: "ZONAS_RISCO");
        }
    }
}
