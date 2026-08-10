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

        if (fechaInicio < DateTime.Now)
        {
            throw new InvalidOperationException(
                "No se puede crear una reserva en el pasado.");
        }

        if (fechaInicio.Hour < 8 || fechaFin.Hour > 18)
        {
            throw new InvalidOperationException(
                "Las reservas solo pueden realizarse entre las 8:00 AM y las 6:00 PM.");
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
        if (Estado == EstadoReserva.Completada)
        {
            throw new InvalidOperationException("Una reserva completada no puede cancelarse.");
        }

        if (Estado == EstadoReserva.Cancelada)
        {
            throw new InvalidOperationException("La reserva ya está cancelada");
        }

        if (FechaInicio <= DateTime.Now.AddHours(2))
        {
            throw new InvalidOperationException("La reserva debe cancelarse con al menos 2 horas de anticipación.");
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
