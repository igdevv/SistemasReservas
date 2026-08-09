# Sistema de Reservas

API REST desarrollada con ASP.NET Core y Onion Architecture para gestionar reservas aplicando reglas de negocio reales.

El objetivo principal de este proyecto es practicar arquitectura de software, separación de responsabilidades y lógica de negocio más allá de un CRUD tradicional.

---

## Objetivo del proyecto

Este sistema busca manejar reservas aplicando reglas reales del negocio, como:

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