# 1. Contexto y definición del problema

## Problemática de negocio

La asignación de mesas en el restaurante se resuelve todavía a mano, por
teléfono o por mensajería, sin dejar constancia de quién confirmó cada
compromiso. Esto genera:

| # | Problema | Descripción |
|---|----------|-------------|
| 1 | Superposición | Una mesa comprometida dos veces en el mismo horario. |
| 2 | Sin respaldo | Nadie sabe quién tomó la reserva. |
| 3 | Dispersión | Cuadernos, llamadas, memoria del encargado de turno. |
| 4 | Mal uso | Mesas libres que parecen ocupadas por falta de registro claro. |

## Alcance de esta entrega

Esta memoria documenta y construye el mantenedor de reservas — **alta,
consulta, modificación y cancelación** — garantizando que ninguna mesa quede
comprometida dos veces.

## Funcionamiento del sistema

1. **Solicitud**: el comensal solicita una mesa para una fecha y hora
   determinadas.
2. **Registro**: el encargado de sala (o administración) registra la
   solicitud en el sistema.
3. **Comprobación**: el sistema comprueba que la mesa esté disponible en ese
   horario — sin cruce con otra reserva confirmada.
4. **Confirmación**: la reserva queda confirmada sin posibilidad de cruce
   con otra.

El mantenedor cubre el ciclo completo de la reserva.

## Perfiles de usuario

| Capacidad | Encargado de sala | Administración |
|-----------|:---:|:---:|
| Registrar y modificar reservas | ✔ | ✔ |
| Consultar disponibilidad diaria | ✔ | ✔ |
| Cancelar reservas | ✔ | ✔ |
| Mantener el catálogo de mesas | — | ✔ |
| Gestionar usuarios y perfiles | — | ✔ |

Administración reúne todas las capacidades del encargado de sala, más el
gobierno del catálogo de mesas/sectores y de los usuarios. El sistema no
muestra opciones que el usuario no puede ejecutar (ver `SesionActual` en la
capa BLL, `docs/06-arquitectura.md`).

## Principio rector

> **Ninguna mesa admite dos reservas que se crucen.**

Si una mesa ya tiene una reserva activa en un horario que se traslapa con el
solicitado, el sistema rechaza la nueva reserva. Se defiende en varias
capas:

| Capa | Defensa |
|------|---------|
| UI | El filtro por fecha evita ofrecer, a simple vista, una mesa ya ocupada en ese horario. |
| BLL | `ReservaBLL` revalida el cruce localmente antes de invocar el DAL. |
| Procedimiento almacenado | `sp_Reserva_Insertar` / `sp_Reserva_Actualizar` rechazan la operación con `RAISERROR` si hay traslape. |
| Auditoría | Todo intento de reserva duplicada, aceptado o rechazado, se conserva en la bitácora. |

La cancelación es **baja lógica**: la reserva no se elimina, queda con
estado `CANCELADA`.
