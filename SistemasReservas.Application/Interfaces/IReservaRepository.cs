using SistemasReservas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Application.Interfaces;

public interface IReservaRepository
{
    Task<bool> ExisteSolapamientoAsync(DateTime inicio, DateTime fin);

    Task AgregarAsync(Reserva reserva);

    Task<List<Reserva>> ObtenerTodasAsync();

    Task<Reserva?> ObtenerPorId(int id);

    Task<Reserva?> ObtenerParaActualizarAsync(int id);

    Task GuardarCambiosAsync();
}
