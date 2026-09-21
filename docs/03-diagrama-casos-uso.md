# 3. Diagrama de Casos de Uso (UML)

```mermaid
flowchart LR
    Encargado(["Encargado de sala"])
    Admin(["Administración"])

    subgraph Sistema["Sistema de Administración de Reservas"]
        UC1(("RF-01 Mantener Sectores y Mesas"))
        UC2(("RF-02 Registrar Comensal"))
        UC3(("RF-03 Registrar Reserva"))
        UC4(("RF-04 Consultar Disponibilidad / Reservas"))
        UC5(("RF-05 Modificar Reserva"))
        UC6(("RF-06 Cancelar Reserva"))
    end

    Admin --> UC1
    Admin --> UC2
    Admin --> UC3
    Admin --> UC4
    Admin --> UC5
    Admin --> UC6

    Encargado --> UC2
    Encargado --> UC3
    Encargado --> UC4
    Encargado --> UC5
    Encargado --> UC6

    UC3 -. incluye .-> UC4
    UC5 -. revalida disponibilidad .-> UC3
```

## Notas de modelado

- **Administración** reúne todas las capacidades de **Encargado de sala**
  (herencia de actor), más `RF-01 Mantener Sectores y Mesas`.
- **RF-03 Registrar Reserva** incluye `RF-04 Consultar Disponibilidad`: no
  se puede confirmar una reserva sin comprobar antes que la mesa esté libre.
- **RF-05 Modificar Reserva** reutiliza la misma validación de traslape que
  `RF-03`, excluyendo la propia reserva del chequeo.
