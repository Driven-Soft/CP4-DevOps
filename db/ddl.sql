CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `TIPOS_OCORRENCIA` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` varchar(60) CHARACTER SET utf8mb4 NOT NULL,
    `Chave` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
    `Descricao` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Ativo` tinyint(1) NOT NULL,
    CONSTRAINT `PK_TIPOS_OCORRENCIA` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `USUARIOS` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` varchar(120) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(180) CHARACTER SET utf8mb4 NOT NULL,
    `Senha` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Telefone` varchar(20) CHARACTER SET utf8mb4 NULL,
    `TipoUsuario` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `Ativo` tinyint(1) NOT NULL,
    `UltimoAcessoEm` datetime(6) NULL,
    CONSTRAINT `PK_USUARIOS` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ZONAS_RISCO` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` varchar(120) CHARACTER SET utf8mb4 NOT NULL,
    `Regiao` varchar(40) CHARACTER SET utf8mb4 NULL,
    `Cidade` varchar(120) CHARACTER SET utf8mb4 NOT NULL,
    `Estado` char(2) CHARACTER SET utf8mb4 NOT NULL,
    `Latitude` double NOT NULL,
    `Longitude` double NOT NULL,
    `Descricao` varchar(300) CHARACTER SET utf8mb4 NULL,
    `NivelRiscoAtual` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Ativa` tinyint(1) NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `AtualizadoEm` datetime(6) NULL,
    CONSTRAINT `PK_ZONAS_RISCO` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ALERTAS` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Titulo` varchar(160) CHARACTER SET utf8mb4 NOT NULL,
    `Descricao` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    `NivelAlerta` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `ZonaRiscoId` int NOT NULL,
    `UsuarioCriadorId` int NULL,
    `InicioVigencia` datetime(6) NULL,
    `FimVigencia` datetime(6) NULL,
    `Ativo` tinyint(1) NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_ALERTAS` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ALERTAS_USUARIOS_UsuarioCriadorId` FOREIGN KEY (`UsuarioCriadorId`) REFERENCES `USUARIOS` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_ALERTAS_ZONAS_RISCO_ZonaRiscoId` FOREIGN KEY (`ZonaRiscoId`) REFERENCES `ZONAS_RISCO` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `OCORRENCIAS` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Titulo` varchar(160) CHARACTER SET utf8mb4 NOT NULL,
    `Descricao` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    `TipoOcorrenciaId` int NOT NULL,
    `NivelRisco` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `UsuarioId` int NOT NULL,
    `ZonaRiscoId` int NULL,
    `Bairro` varchar(120) CHARACTER SET utf8mb4 NULL,
    `Latitude` double NOT NULL,
    `Longitude` double NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `DataAtualizacao` datetime(6) NULL,
    `ResolvidoEm` datetime(6) NULL,
    CONSTRAINT `PK_OCORRENCIAS` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_OCORRENCIAS_TIPOS_OCORRENCIA_TipoOcorrenciaId` FOREIGN KEY (`TipoOcorrenciaId`) REFERENCES `TIPOS_OCORRENCIA` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_OCORRENCIAS_USUARIOS_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `USUARIOS` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_OCORRENCIAS_ZONAS_RISCO_ZonaRiscoId` FOREIGN KEY (`ZonaRiscoId`) REFERENCES `ZONAS_RISCO` (`Id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

CREATE TABLE `LOGS_ALERTA` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AlertaId` int NOT NULL,
    `UsuarioId` int NOT NULL,
    `Acao` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `DadosAntes` longtext CHARACTER SET utf8mb4 NULL,
    `DadosDepois` longtext CHARACTER SET utf8mb4 NULL,
    `DataCriacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_LOGS_ALERTA` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LOGS_ALERTA_ALERTAS_AlertaId` FOREIGN KEY (`AlertaId`) REFERENCES `ALERTAS` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LOGS_ALERTA_USUARIOS_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `USUARIOS` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `COMENTARIOS_OCORRENCIA` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Mensagem` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
    `OcorrenciaId` int NOT NULL,
    `UsuarioId` int NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `Ativo` tinyint(1) NOT NULL,
    CONSTRAINT `PK_COMENTARIOS_OCORRENCIA` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_COMENTARIOS_OCORRENCIA_OCORRENCIAS_OcorrenciaId` FOREIGN KEY (`OcorrenciaId`) REFERENCES `OCORRENCIAS` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_COMENTARIOS_OCORRENCIA_USUARIOS_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `USUARIOS` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_ALERTAS_Ativo` ON `ALERTAS` (`Ativo`);

CREATE INDEX `IX_ALERTAS_NivelAlerta` ON `ALERTAS` (`NivelAlerta`);

CREATE INDEX `IX_ALERTAS_UsuarioCriadorId` ON `ALERTAS` (`UsuarioCriadorId`);

CREATE INDEX `IX_ALERTAS_ZonaRiscoId` ON `ALERTAS` (`ZonaRiscoId`);

CREATE INDEX `IX_COMENTARIOS_OCORRENCIA_OcorrenciaId_DataCriacao` ON `COMENTARIOS_OCORRENCIA` (`OcorrenciaId`, `DataCriacao`);

CREATE INDEX `IX_COMENTARIOS_OCORRENCIA_UsuarioId` ON `COMENTARIOS_OCORRENCIA` (`UsuarioId`);

CREATE INDEX `IX_LOGS_ALERTA_AlertaId` ON `LOGS_ALERTA` (`AlertaId`);

CREATE INDEX `IX_LOGS_ALERTA_UsuarioId` ON `LOGS_ALERTA` (`UsuarioId`);

CREATE INDEX `IX_OCORRENCIAS_DataCriacao` ON `OCORRENCIAS` (`DataCriacao` DESC);

CREATE INDEX `IX_OCORRENCIAS_Status` ON `OCORRENCIAS` (`Status`);

CREATE INDEX `IX_OCORRENCIAS_TipoOcorrenciaId` ON `OCORRENCIAS` (`TipoOcorrenciaId`);

CREATE INDEX `IX_OCORRENCIAS_UsuarioId` ON `OCORRENCIAS` (`UsuarioId`);

CREATE INDEX `IX_OCORRENCIAS_ZonaRiscoId` ON `OCORRENCIAS` (`ZonaRiscoId`);

CREATE UNIQUE INDEX `IX_TIPOS_OCORRENCIA_Chave` ON `TIPOS_OCORRENCIA` (`Chave`);

CREATE UNIQUE INDEX `IX_USUARIOS_Email` ON `USUARIOS` (`Email`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260826190936_InitialCreate', '9.0.14');

COMMIT;

