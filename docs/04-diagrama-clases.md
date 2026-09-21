# 4. Diagrama de Clases (UML)

Refleja las entidades reales implementadas en
`src/SistemaReservasRestaurante.Entidades`, alineadas con
`database/schema.sql`.

```mermaid
classDiagram
    class Sector {
        +int SectorId
        +string Nombre
    }

    class Mesa {
        +int MesaId
        +int Numero
        +int Capacidad
        +int SectorId
        +string Estado
        +string Etiqueta()
    }

    class Comensal {
        +int ComensalId
        +string Nombre
        +string Telefono
        +string Email
    }

    class Reserva {
        +int ReservaId
        +int ComensalId
        +int MesaId
        +DateTime FechaHoraInicio
        +DateTime FechaHoraFin
        +int CantidadPersonas
        +EstadoReserva Estado
        +int AutorUsuarioId
        +DateTime FechaCreacion
        +DateTime FechaCancelacion
    }

    class EstadoReserva {
        <<enumeration>>
        CONFIRMADA
        CANCELADA
    }

    class Usuario {
        +int UsuarioId
        +string NombreUsuario
        +int RolId
        +RolUsuario Rol
        +string Estado
    }

    class RolUsuario {
        <<enumeration>>
        ENCARGADO
        ADMINISTRACION
    }

    class RegistroAuditoria {
        +int AuditoriaId
        +int UsuarioId
        +string Accion
        +string Entidad
        +int EntidadId
        +string Detalle
        +DateTime FechaHora
        +string OrigenEquipo
    }

    Sector "1" --> "0..*" Mesa : agrupa
    Comensal "1" --> "0..*" Reserva : origina
    Mesa "1" --> "0..*" Reserva : recibe
    Reserva ..> EstadoReserva : usa
    Usuario "1" --> "0..*" Reserva : registra como autor
    Usuario ..> RolUsuario : usa
    Usuario "1" --> "0..*" RegistroAuditoria : genera
```

## Decisiones de diseño reflejadas en el modelo

- **`Sector` como catálogo aparte de `Mesa`**: interior, terraza o salón
  privado se administran en una tabla propia, no como texto libre repetido
  en cada mesa.
- **`Reserva` es N:1 con `Mesa` y N:1 con `Comensal`**: una mesa puede tener
  muchas reservas a lo largo del tiempo (mientras no se traslapen), y un
  comensal puede reservar muchas veces.
- **`EstadoReserva` como enumeración**, no texto libre: el compilador
  impide asignar un estado inválido desde BLL/UI; el `CHECK` en SQL hace lo
  mismo del lado de la base de datos.
- **`FechaHoraInicio` / `FechaHoraFin`** como columnas separadas (en vez de
  una sola fecha + duración) permiten expresar directamente la condición de
  traslape como una comparación de rangos, tanto en C# como en SQL.
