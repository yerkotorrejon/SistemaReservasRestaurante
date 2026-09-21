-- ============================================================
-- Sistema de Administración de Reservas para Restaurante
-- Script DDL: creación de base de datos, tablas, PKs, FKs
-- Motor: SQL Server Express / LocalDB
-- ============================================================

IF DB_ID('ReservasRestauranteDB') IS NULL
BEGIN
    CREATE DATABASE ReservasRestauranteDB;
END
GO

USE ReservasRestauranteDB;
GO

-- ------------------------------------------------------------
-- Seguridad: Rol, Usuario, RegistroAuditoria
-- ------------------------------------------------------------
CREATE TABLE Rol (
    RolId       INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(30)  NOT NULL UNIQUE
);
GO

CREATE TABLE Usuario (
    UsuarioId       INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario   VARCHAR(50)   NOT NULL UNIQUE,
    PasswordHash    VARBINARY(64) NOT NULL,
    Salt            VARBINARY(32) NOT NULL,
    RolId           INT           NOT NULL REFERENCES Rol(RolId),
    Estado          VARCHAR(15)   NOT NULL DEFAULT 'ACTIVO',
    UltimoAcceso    DATETIME2     NULL,
    CONSTRAINT CK_Usuario_Estado CHECK (Estado IN ('ACTIVO','BLOQUEADO'))
);
GO

CREATE TABLE RegistroAuditoria (
    AuditoriaId     INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId       INT           NOT NULL REFERENCES Usuario(UsuarioId),
    Accion          VARCHAR(50)   NOT NULL,   -- LECTURA / INSERCION / ACTUALIZACION / CANCELACION / BLOQUEO
    Entidad         VARCHAR(50)   NOT NULL,   -- Reserva, Mesa, etc.
    EntidadId       INT           NULL,
    Detalle         VARCHAR(500)  NULL,
    FechaHora       DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    OrigenEquipo    VARCHAR(100)  NULL
);
GO

-- ------------------------------------------------------------
-- Dominio del restaurante
-- ------------------------------------------------------------
CREATE TABLE Sector (
    SectorId    INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE Mesa (
    MesaId      INT IDENTITY(1,1) PRIMARY KEY,
    Numero      INT          NOT NULL UNIQUE,
    Capacidad   INT          NOT NULL,
    SectorId    INT          NOT NULL REFERENCES Sector(SectorId),
    Estado      VARCHAR(15)  NOT NULL DEFAULT 'ACTIVA',
    CONSTRAINT CK_Mesa_Capacidad CHECK (Capacidad > 0),
    CONSTRAINT CK_Mesa_Estado CHECK (Estado IN ('ACTIVA','INACTIVA'))
);
GO

CREATE TABLE Comensal (
    ComensalId  INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(120) NOT NULL,
    Telefono    VARCHAR(20)  NOT NULL,
    Email       VARCHAR(100) NULL
);
GO

-- Entidad principal: vincula comensal + mesa en un horario, con estado
CREATE TABLE Reserva (
    ReservaId           INT IDENTITY(1,1) PRIMARY KEY,
    ComensalId          INT          NOT NULL REFERENCES Comensal(ComensalId),
    MesaId              INT          NOT NULL REFERENCES Mesa(MesaId),
    FechaHoraInicio      DATETIME2    NOT NULL,
    FechaHoraFin         DATETIME2    NOT NULL,
    CantidadPersonas    INT          NOT NULL,
    Estado              VARCHAR(15)  NOT NULL DEFAULT 'CONFIRMADA',
    AutorUsuarioId      INT          NOT NULL REFERENCES Usuario(UsuarioId),
    FechaCreacion       DATETIME2    NOT NULL DEFAULT SYSDATETIME(),
    FechaCancelacion    DATETIME2    NULL,
    CONSTRAINT CK_Reserva_Horario CHECK (FechaHoraFin > FechaHoraInicio),
    CONSTRAINT CK_Reserva_Personas CHECK (CantidadPersonas > 0),
    CONSTRAINT CK_Reserva_Estado CHECK (Estado IN ('CONFIRMADA','CANCELADA'))
);
GO

CREATE INDEX IX_Reserva_Mesa    ON Reserva(MesaId);
CREATE INDEX IX_Reserva_Comensal ON Reserva(ComensalId);
CREATE INDEX IX_Mesa_Sector     ON Mesa(SectorId);
GO

-- Datos base mínimos
INSERT INTO Rol (Nombre) VALUES ('ENCARGADO'), ('ADMINISTRACION');
INSERT INTO Sector (Nombre) VALUES ('Interior'), ('Terraza'), ('Salón privado');
GO
