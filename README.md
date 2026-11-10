# Sistema de Reservas

API REST educativa desarrollada con ASP.NET Core 10 y Onion Architecture para gestionar reservas, estados y validaciones de horarios. Este repositorio demuestra separación de responsabilidades y reglas de dominio; no se presenta como un sistema listo para producción.

El objetivo principal de este proyecto es practicar arquitectura de software, separación de responsabilidades y lógica de negocio más allá de un CRUD tradicional.

---

## Estado actual

La API implementa creación, consulta y confirmación de reservas. Comprueba solapamientos antes de guardar, pero **esa comprobación por sí sola no garantiza evitar reservas simultáneas bajo concurrencia**. Cancelación, finalización, recordatorios, trabajos en segundo plano, autenticación y protección robusta frente a dobles reservas siguen siendo mejoras pendientes.

## Objetivo del proyecto

El objetivo a largo plazo es manejar reservas con reglas de negocio como las siguientes (no todas están implementadas):

- Evitar reservas solapadas.
- Manejar estados de reserva.
- Aplicar reglas de validación.
- Confirmar reservas.
- Cancelar reservas.
- Completar reservas.
- Trabajar con horarios.
- Implementar recordatorios.
- Ejecutar background jobs.
- Enviar correos electrónicos.
- Manejar concurrencia para evitar dobles reservas.

---

## Tecnologías

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- Onion Architecture

---

## Arquitectura

El proyecto utiliza Onion Architecture.

La idea principal es que las dependencias apunten hacia las capas internas.

```text
API
 │
 ▼
Application
 │
 ▼
Domain

Infrastructure
 │
 └── implementa contratos definidos por Application
```

---

## Domain

`SistemasReservas.Domain`

Es el núcleo del sistema.

Aquí viven las entidades y las reglas principales del negocio.

Actualmente contiene:

- `Reserva`
- `EstadoReserva`

Ejemplos de reglas de dominio:

- Una reserva no puede terminar antes de comenzar.
- Toda reserva nueva comienza en estado `Pendiente`.
- Solo una reserva pendiente puede confirmarse.
- Una reserva completada no puede cancelarse.
- Una reserva cancelada no puede volver a confirmarse.

Ejemplo:

```csharp
reserva.Confirmar();
```

La propia entidad decide si esa operación está permitida. Las reservas deben comenzar desde las 08:00, terminar como máximo a las 18:00 y mantenerse dentro del mismo día. La validación utiliza la hora local representada por los valores `DateTime` recibidos; todavía no existe una política explícita de zonas horarias.

---

## Application

`SistemasReservas.Application`

Contiene los casos de uso de la aplicación.

Actualmente se encarga de:

- Crear reservas.
- Obtener todas las reservas.
- Obtener una reserva por Id.
- Confirmar reservas.
- Verificar si existe solapamiento.

También contiene contratos como:

```csharp
IReservaRepository
```

Application define lo que necesita, pero no sabe cómo se accede a SQL Server.

Ejemplo:

```text
Application:
"Necesito saber si existe una reserva en este horario."

Infrastructure:
"Yo sé cómo consultarlo."
```

---

## Infrastructure

`SistemasReservas.Infrastructure`

Contiene los detalles técnicos.

Actualmente incluye:

- Entity Framework Core.
- SQL Server.
- `ApplicationDbContext`.
- `ReservaRepository`.
- Persistencia de reservas.

Ejemplo:

```csharp
public class ReservaRepository : IReservaRepository
```

Application define el contrato.

Infrastructure implementa ese contrato.

---

## API

`SistemasReservas.API`

Es la entrada HTTP de la aplicación.

Aquí viven:

- Controllers.
- Configuración de Dependency Injection.
- Configuración de SQL Server.
- Swagger.

El Controller recibe peticiones HTTP y delega el trabajo a Application.

---

## Flujo de creación de una reserva

```text
Cliente
   │
   ▼
POST /api/Reservas
   │
   ▼
ReservasController
   │
   ▼
ReservaService
   │
   ▼
IReservaRepository
   │
   ▼
ReservaRepository
   │
   ▼
SQL Server
```

Antes de guardar una reserva, Application verifica que no exista otra reserva solapada.

Luego Domain valida que la reserva sea válida.

---

## Estados de una reserva

El sistema utiliza los siguientes estados:

- Pendiente
- Confirmada
- Cancelada
- Completada

Flujo permitido:

```text
Pendiente
   ├── Confirmada
   └── Cancelada

Confirmada
   ├── Completada
   └── Cancelada

Cancelada
   └── Estado final

Completada
   └── Estado final
```

---

## Prevención de reservas solapadas

Antes de crear una reserva se verifica si existe otra reserva activa que choque con el horario solicitado. Las reservas canceladas no bloquean el horario. Esta comprobación no sustituye una garantía transaccional contra solicitudes concurrentes.

La condición utilizada es:

```csharp
inicio < reserva.FechaFin &&
fin > reserva.FechaInicio
```

Ejemplo:

```text
Reserva existente:
10:00 -------- 11:00

Nueva reserva:
       10:30 -------- 11:30

Resultado:
Solapamiento detectado
```

---

## DTOs

La API no expone directamente las entidades del dominio.

Actualmente se utilizan:

- `CrearReservaRequest`
- `ReservaResponse`

Flujo:

```text
Cliente
   │
   ▼
CrearReservaRequest
   │
   ▼
Application
   │
   ▼
Reserva
   │
   ▼
ReservaResponse
   │
   ▼
Cliente
```

---

## Ejecución local

Requiere .NET SDK 10 y SQL Server o LocalDB. Configura `ConnectionStrings__DefaultConnection` mediante variable de entorno o User Secrets, sin publicar credenciales. Desde la raíz del repositorio:

```powershell
dotnet restore SistemasReservas.slnx
dotnet build SistemasReservas.slnx
dotnet ef database update --project SistemasReservas.Infrastructure --startup-project SistemasReservas.API
dotnet run --project SistemasReservas.API
```

En `Development`, abre `/swagger` en la dirección local indicada por la aplicación. Revisa las migraciones antes de aplicarlas a una base con datos. El repositorio incluye un workflow de GitHub Actions que compila la solución; todavía no contiene un proyecto de pruebas automatizadas.

## Endpoints actuales

### Crear reserva

```http
POST /api/Reservas
```

Devuelve **201 Created** y una cabecera `Location` hacia la reserva creada.

Ejemplo:

```json
{
  "fechaInicio": "2026-08-10T10:00:00",
  "fechaFin": "2026-08-10T11:00:00"
}
```

### Obtener todas las reservas

```http
GET /api/Reservas
```

### Obtener reserva por Id

```http
GET /api/Reservas/{id}
```

### Confirmar reserva

```http
PATCH /api/Reservas/{id}/confirmar
```

---

## Base de datos

El proyecto utiliza SQL Server mediante Entity Framework Core.

La persistencia se encuentra dentro de:

`SistemasReservas.Infrastructure`

El DbContext principal es:

`ApplicationDbContext`

---

### Cancelar reserva

```http
PATCH /api/Reservas/{id}/cancelar
```

El dominio impide cancelar reservas completadas, ya canceladas o que comienzan dentro de las siguientes dos horas. El método `Completar()` existe en la entidad, pero todavía no se expone como endpoint HTTP.

## Próximas funcionalidades

- Completar reservas.
- Reglas de horario laboral.
- Restricciones de cancelación.
- Manejo global de errores.
- Problem Details.
- Validaciones.
- Concurrencia.
- Prevención robusta de doble reserva.
- Transacciones.
- Background Jobs.
- Recordatorios automáticos.
- Envío de correos electrónicos.
- Autenticación JWT.
- Roles y autorización.
- Unit Tests.
- Integration Tests.
- Docker.
- Despliegue en Azure.
- CI/CD.

---

## Objetivo de aprendizaje

Este proyecto fue creado para practicar backend e ingeniería de software.

Conceptos trabajados:

- Onion Architecture.
- Dependency Injection.
- Dependency Inversion.
- Repository Pattern.
- Separación de responsabilidades.
- Domain Logic.
- Application Services.
- Entity Framework Core.
- Manejo de estados.
- Reglas de negocio.
- DTOs.
- SQL Server.

---

## Autor

José Ignacio Suero
