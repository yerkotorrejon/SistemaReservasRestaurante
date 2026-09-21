# 2. Especificación de requerimientos (Tríada + ISO/IEC 27001)

Cada requerimiento funcional (RF) se define mediante la tríada:
**Rol/Usuario + Necesidad/Acción + Criterio de Aceptación/Seguridad**, e
incorpora los controles de seguridad de la norma ISO/IEC 27001 (Anexo A)
que aplica.

---

### RF-01 · Mantención de sectores y mesas

| Tríada | Detalle |
|---|---|
| **Rol/Usuario** | Administración |
| **Necesidad/Acción** | Registrar, editar e inactivar mesas y su sector. |
| **Criterio/Seguridad** | Número de mesa único, capacidad mayor a cero, sector existente. |

**ISO/IEC 27001**: A.8.28 Codificación segura · A.5.33 Protección de registros.

---

### RF-02 · Registro de comensal

| Tríada | Detalle |
|---|---|
| **Rol/Usuario** | Encargado de sala o Administración |
| **Necesidad/Acción** | Registrar un nuevo comensal para poder asociarlo a una reserva. |
| **Criterio/Seguridad** | Nombre y teléfono obligatorios; correo con formato válido si se ingresa. |

**ISO/IEC 27001**: A.8.28 Codificación segura.

---

### RF-03 · Registro de reserva

| Tríada | Detalle |
|---|---|
| **Rol/Usuario** | Encargado de sala |
| **Necesidad/Acción** | Registrar una reserva asociada a un comensal y a una mesa. |
| **Criterio/Seguridad** | Mesa disponible en el horario solicitado (sin traslape); autor y fecha-hora registrados automáticamente; capacidad de la mesa no excedida. |

> Como encargado de sala, requiero registrar una reserva asociada a un
> comensal y a una mesa, de modo que el sistema compruebe la disponibilidad
> en el horario solicitado y registre autor y fecha-hora de forma
> automática.

**ISO/IEC 27001**: A.5.15 Control de acceso · A.8.15 Registro de eventos.

---

### RF-04 · Consulta de disponibilidad y reservas

| Tríada | Detalle |
|---|---|
| **Rol/Usuario** | Encargado de sala o Administración |
| **Necesidad/Acción** | Consultar las reservas de una fecha, mesa o comensal. |
| **Criterio/Seguridad** | El listado se puede filtrar por fecha; toda lectura queda disponible al instante tras guardarse. |

**ISO/IEC 27001**: A.5.15 Control de acceso.

---

### RF-05 · Modificación de reserva

| Tríada | Detalle |
|---|---|
| **Rol/Usuario** | Encargado de sala |
| **Necesidad/Acción** | Cambiar mesa, horario o cantidad de personas de una reserva vigente. |
| **Criterio/Seguridad** | Revalida disponibilidad si cambia mesa u horario (excluyendo la propia reserva); rechazada si la reserva ya está `CANCELADA`. |

**ISO/IEC 27001**: A.5.15 Control de acceso · A.8.15 Registro de eventos.

---

### RF-06 · Cancelación de reserva (baja lógica)

| Tríada | Detalle |
|---|---|
| **Rol/Usuario** | Encargado de sala |
| **Necesidad/Acción** | Cancelar una reserva que ya no se concretará. |
| **Criterio/Seguridad** | Cambia el estado a `CANCELADA`; **nunca** se ejecuta un `DELETE` físico; queda auditado quién y cuándo. |

**ISO/IEC 27001**: A.5.33 Protección de registros · A.8.15 Registro de eventos.

---

## Requerimientos no funcionales (RNF)

| # | RNF | Descripción | ISO/IEC 27001 |
|---|-----|-------------|----------------|
| RNF-01 | Control de acceso por rol | Solo Administración mantiene el catálogo de mesas/sectores y usuarios; Encargado gestiona reservas. | A.5.15 Control de acceso |
| RNF-02 | Autenticación de sesión | La sesión activa queda identificada por un `Usuario` válido antes de ejecutar cualquier operación de negocio. | A.5.17 Autenticación |
| RNF-03 | Registro de auditoría | Toda inserción, actualización, cancelación e intento de reserva duplicada (aceptado o rechazado) queda registrado en `RegistroAuditoria`. | A.8.15 Registro de eventos |
| RNF-04 | Protección de registros | Ninguna reserva se elimina físicamente; toda baja es lógica (`CANCELADA`). | A.5.33 Protección de registros |
| RNF-05 | Acceso a datos exclusivamente vía procedimientos almacenados | Ninguna capa ejecuta SQL dinámico ni concatenado; todo pasa por SPs con parámetros tipados. | A.8.28 Codificación segura |
| RNF-06 | Integridad referencial | Claves foráneas y `CHECK` constraints impiden estados o relaciones inconsistentes incluso si una capa superior falla. | A.8.28 Codificación segura |

## Trazabilidad de seguridad (resumen)

| Entidad | Rol ISO 27001 |
|---------|----------------|
| `Usuario` | Credenciales, estado, último acceso → A.5.17 Autenticación |
| `Rol` / `SesionActual` | Permisos condicionados por rol → A.5.15 Control de acceso |
| `RegistroAuditoria` | Quién leyó o modificó qué, cuándo y desde dónde → A.8.15 Registro de eventos |
| `Reserva.Estado` + SPs | Sin cruces de mesa, sin DELETE físico → A.5.33 Protección de registros |

El control de acceso evita que una reserva se cancele sin que quede
identificado quién lo hizo. Cada intento de reserva duplicada, aceptado o
rechazado, se conserva en la bitácora de auditoría.
