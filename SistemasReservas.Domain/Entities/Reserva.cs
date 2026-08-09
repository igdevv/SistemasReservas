using SistemasReservas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Domain.Entities;

public class Reserva
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoReserva Estado { get; private set; }

    public Reserva(DateTime fechaInicio, DateTime fechaFin)
    {
        if (fechaFin <= fechaInicio)
        {
            throw new ArgumentException("La fecha de fin debe ser mayor que la fecha de inicio.");
        }

        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        Estado = EstadoReserva.Pendiente;
    }

    public void Confirmar()
    {
        if (Estado != EstadoReserva.Pendiente)
        {
            throw new InvalidOperationException("Solo una reserva pendiente puede ser confirmada.");
        }

        Estado = EstadoReserva.Confirmada;
    }

    public void Cancelar()
    {
        if (Estado == EstadoReserva.Cancelada)
        {
            throw new InvalidOperationException("La reserva ya está cancelada");
        }

        Estado = EstadoReserva.Cancelada;
    }

    public void Completar()
    {
        if (Estado != EstadoReserva.Confirmada)
        {
            throw new InvalidOperationException("Solo una reserva confirmada puede completarse");
        }

        Estado = EstadoReserva.Completada;
    }
}
