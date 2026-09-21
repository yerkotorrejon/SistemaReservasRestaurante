-- ============================================================
-- Sistema de Administración de Reservas para Restaurante
-- Stored Procedures: CRUD completo por entidad
-- Ninguna sentencia SQL se ejecuta directo desde C#; todo pasa por aquí.
-- ============================================================

USE ReservasRestauranteDB;
GO

-- ============================================================
-- SECTOR
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Sector_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SectorId, Nombre FROM Sector ORDER BY Nombre;
END
GO

-- ============================================================
-- MESA
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Mesa_Insertar
    @Numero INT, @Capacidad INT, @SectorId INT,
    @MesaId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Mesa WHERE Numero = @Numero)
    BEGIN
        RAISERROR('Ya existe una mesa con ese número.', 16, 1);
        RETURN;
    END

    INSERT INTO Mesa (Numero, Capacidad, SectorId)
    VALUES (@Numero, @Capacidad, @SectorId);

    SET @MesaId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Mesa_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.MesaId, m.Numero, m.Capacidad, m.SectorId, s.Nombre AS Sector, m.Estado
    FROM Mesa m
    JOIN Sector s ON s.SectorId = m.SectorId
    WHERE m.Estado = 'ACTIVA'
    ORDER BY m.Numero;
END
GO

CREATE OR ALTER PROCEDURE sp_Mesa_Actualizar
    @MesaId INT, @Capacidad INT, @SectorId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Mesa
    SET Capacidad = @Capacidad, SectorId = @SectorId
    WHERE MesaId = @MesaId AND Estado = 'ACTIVA';

    IF @@ROWCOUNT = 0
        RAISERROR('No se pudo actualizar: la mesa no existe o está inactiva.', 16, 1);
END
GO

CREATE OR ALTER PROCEDURE sp_Mesa_Anular
    @MesaId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Mesa SET Estado = 'INACTIVA' WHERE MesaId = @MesaId;
    IF @@ROWCOUNT = 0
        RAISERROR('Mesa no encontrada.', 16, 1);
END
GO

-- ============================================================
-- COMENSAL
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Comensal_Insertar
    @Nombre VARCHAR(120), @Telefono VARCHAR(20), @Email VARCHAR(100),
    @ComensalId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Comensal (Nombre, Telefono, Email)
    VALUES (@Nombre, @Telefono, @Email);

    SET @ComensalId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Comensal_Listar
    @Filtro VARCHAR(120) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ComensalId, Nombre, Telefono, Email
    FROM Comensal
    WHERE @Filtro IS NULL OR Nombre LIKE '%' + @Filtro + '%' OR Telefono LIKE '%' + @Filtro + '%'
    ORDER BY Nombre;
END
GO

-- ============================================================
-- RESERVA (entidad principal — principio rector: sin cruces de mesa)
-- ============================================================

-- Rechaza el alta si la mesa ya tiene una reserva CONFIRMADA cuyo horario se traslapa
CREATE OR ALTER PROCEDURE sp_Reserva_Insertar
    @ComensalId INT, @MesaId INT, @FechaHoraInicio DATETIME2, @FechaHoraFin DATETIME2,
    @CantidadPersonas INT, @AutorUsuarioId INT,
    @ReservaId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Mesa WHERE MesaId = @MesaId AND Estado = 'ACTIVA')
    BEGIN
        RAISERROR('La mesa indicada no existe o está inactiva.', 16, 1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1 FROM Reserva
        WHERE MesaId = @MesaId
          AND Estado = 'CONFIRMADA'
          AND @FechaHoraInicio < FechaHoraFin
          AND @FechaHoraFin > FechaHoraInicio
    )
    BEGIN
        RAISERROR('La mesa ya tiene una reserva confirmada que se traslapa con ese horario.', 16, 1);
        RETURN;
    END

    INSERT INTO Reserva (ComensalId, MesaId, FechaHoraInicio, FechaHoraFin, CantidadPersonas, Estado, AutorUsuarioId, FechaCreacion)
    VALUES (@ComensalId, @MesaId, @FechaHoraInicio, @FechaHoraFin, @CantidadPersonas, 'CONFIRMADA', @AutorUsuarioId, SYSDATETIME());

    SET @ReservaId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Reserva_Listar
    @Fecha DATE = NULL, @MesaId INT = NULL, @ComensalId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.ReservaId, r.ComensalId, c.Nombre AS Comensal, c.Telefono,
           r.MesaId, m.Numero AS NumeroMesa, r.FechaHoraInicio, r.FechaHoraFin,
           r.CantidadPersonas, r.Estado, r.AutorUsuarioId, r.FechaCreacion
    FROM Reserva r
    JOIN Comensal c ON c.ComensalId = r.ComensalId
    JOIN Mesa m ON m.MesaId = r.MesaId
    WHERE (@Fecha IS NULL OR CAST(r.FechaHoraInicio AS DATE) = @Fecha)
      AND (@MesaId IS NULL OR r.MesaId = @MesaId)
      AND (@ComensalId IS NULL OR r.ComensalId = @ComensalId)
    ORDER BY r.FechaHoraInicio DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Reserva_ObtenerPorId
    @ReservaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ReservaId, ComensalId, MesaId, FechaHoraInicio, FechaHoraFin,
           CantidadPersonas, Estado, AutorUsuarioId, FechaCreacion, FechaCancelacion
    FROM Reserva WHERE ReservaId = @ReservaId;
END
GO

-- Revalida disponibilidad si cambia mesa u horario (excluye la propia reserva)
CREATE OR ALTER PROCEDURE sp_Reserva_Actualizar
    @ReservaId INT, @MesaId INT, @FechaHoraInicio DATETIME2, @FechaHoraFin DATETIME2,
    @CantidadPersonas INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Estado VARCHAR(15);
    SELECT @Estado = Estado FROM Reserva WHERE ReservaId = @ReservaId;

    IF @Estado IS NULL
    BEGIN
        RAISERROR('La reserva no existe.', 16, 1);
        RETURN;
    END
    IF @Estado <> 'CONFIRMADA'
    BEGIN
        RAISERROR('No se puede modificar: la reserva está %s.', 16, 1, @Estado);
        RETURN;
    END

    IF EXISTS (
        SELECT 1 FROM Reserva
        WHERE MesaId = @MesaId
          AND Estado = 'CONFIRMADA'
          AND ReservaId <> @ReservaId
          AND @FechaHoraInicio < FechaHoraFin
          AND @FechaHoraFin > FechaHoraInicio
    )
    BEGIN
        RAISERROR('La mesa ya tiene otra reserva confirmada que se traslapa con ese horario.', 16, 1);
        RETURN;
    END

    UPDATE Reserva
    SET MesaId = @MesaId, FechaHoraInicio = @FechaHoraInicio, FechaHoraFin = @FechaHoraFin,
        CantidadPersonas = @CantidadPersonas
    WHERE ReservaId = @ReservaId;
END
GO

-- Baja lógica: nunca borra físicamente una reserva
CREATE OR ALTER PROCEDURE sp_Reserva_Cancelar
    @ReservaId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Reserva SET Estado = 'CANCELADA', FechaCancelacion = SYSDATETIME()
    WHERE ReservaId = @ReservaId AND Estado = 'CONFIRMADA';

    IF @@ROWCOUNT = 0
        RAISERROR('La reserva no existe o ya estaba cancelada.', 16, 1);
END
GO

-- ============================================================
-- AUDITORIA / USUARIO
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Auditoria_Insertar
    @UsuarioId INT, @Accion VARCHAR(50), @Entidad VARCHAR(50),
    @EntidadId INT = NULL, @Detalle VARCHAR(500) = NULL, @OrigenEquipo VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RegistroAuditoria (UsuarioId, Accion, Entidad, EntidadId, Detalle, FechaHora, OrigenEquipo)
    VALUES (@UsuarioId, @Accion, @Entidad, @EntidadId, @Detalle, SYSDATETIME(), @OrigenEquipo);
END
GO

CREATE OR ALTER PROCEDURE sp_Usuario_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UsuarioId, u.NombreUsuario, u.RolId, r.Nombre AS Rol, u.Estado
    FROM Usuario u
    JOIN Rol r ON r.RolId = u.RolId
    WHERE u.Estado = 'ACTIVO';
END
GO
