# Sistema de Administración de Reservas para Restaurante

Mantenedor para ordenar la ocupación de mesas y dejar constancia de cada
movimiento. C# · .NET 8 Windows Forms · SQL Server · Arquitectura en 4
capas · ISO/IEC 27001.

Documentación completa (contexto, requerimientos, UML, modelo relacional,
arquitectura) en [`/docs`](docs/README.md).

## Estructura del repositorio

```
/docs      → informe técnico completo
/database  → schema.sql (DDL) y sps.sql (Stored Procedures)
/src       → solución C# en 4 capas (Entidades, DAL, BLL, UI)
```

## Requisitos

- .NET 8 SDK
- SQL Server Express o LocalDB (probado contra instancia `.\SQLEXPRESS`)

## Puesta en marcha

1. Ejecutar `database/schema.sql` contra tu instancia SQL Server.
2. Ejecutar `database/sps.sql` contra la base `ReservasRestauranteDB` creada.
3. Verificar la cadena de conexión en
   `src/SistemaReservasRestaurante.DAL/ConexionDb.cs` (por defecto usa
   `.\SQLEXPRESS` con autenticación de Windows).
4. Correr la aplicación:

```bash
cd src/SistemaReservasRestaurante.UI
dotnet run
```

5. En el login, elegir un usuario (`encargado1` o `admin1`). No hay
   contraseña (login simplificado); el sistema sí condiciona cada acción
   por rol una vez dentro — solo `admin1` ve el mantenedor de mesas.
