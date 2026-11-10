using Microsoft.EntityFrameworkCore;
using SistemasReservas.Application.Interfaces;
using SistemasReservas.Domain.Entities;
using SistemasReservas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AgregarAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteSolapamientoAsync(DateTime inicio, DateTime fin)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.Estado != SistemasReservas.Domain.Enums.EstadoReserva.Cancelada &&
                inicio < r.FechaFin && fin > r.FechaInicio);
        }

        public async Task<List<Reserva>> ObtenerTodasAsync()
        {
            return await _context.Reservas.AsNoTracking().ToListAsync();
        }

        public async Task<Reserva?> ObtenerPorId(int id)
        {
            return await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reserva?> ObtenerParaActualizarAsync(int id)
        {
            return await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
