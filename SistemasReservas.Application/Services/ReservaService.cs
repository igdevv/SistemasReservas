using SistemasReservas.Application.DTOs;
using SistemasReservas.Application.Interfaces;
using SistemasReservas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Application.Services;

public class ReservaService
{
    private readonly IReservaRepository _reservaRepository;

    public ReservaService(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }
    
    public async Task<ReservaResponse> CrearReservaAsync(CrearReservaRequest request)
    {
        var existeSolapamiento = await _reservaRepository.ExisteSolapamientoAsync(request.FechaInicio, request.FechaFin);

        if (existeSolapamiento)
        {
            throw new InvalidOperationException("Ya existe una reserva en ese horario");
        }

        var reserva = new Reserva(request.FechaInicio, request.FechaFin);

        await _reservaRepository.AgregarAsync(reserva);

        return new ReservaResponse
        {
            Id = reserva.Id,
            FechaInicio = reserva.FechaInicio,
            FechaFin = reserva.FechaFin,
            Estado = reserva.Estado.ToString()
        };
    }

    public async Task<List<ReservaResponse>> ObtenerTodasAsync()
    {
        var reservas = await _reservaRepository.ObtenerTodasAsync();

        return reservas.Select(reserva => new ReservaResponse
        {
            Id = reserva.Id,
            FechaInicio = reserva.FechaInicio,
            FechaFin = reserva.FechaFin,
            Estado = reserva.Estado.ToString()
        }).ToList();
    }

    public async Task<ReservaResponse?> ObtenerPorIdAsync(int id)
    {
        var reservaPorId = await _reservaRepository.ObtenerPorId(id);

        if (reservaPorId is null)
        {
            return null;
        }

        return new ReservaResponse
        {
            Id = reservaPorId.Id,
            FechaInicio = reservaPorId.FechaInicio,
            FechaFin = reservaPorId.FechaFin,
            Estado = reservaPorId.Estado.ToString()
        };
    }

    public async Task ConfirmarAsync(int id)
    {
        var reserva = await _reservaRepository.ObtenerParaActualizarAsync(id);

        if (reserva == null)
        {
            throw new KeyNotFoundException("La reserva no existe");
        }

        reserva.Confirmar();

        await _reservaRepository.GuardarCambiosAsync();
    }
}
