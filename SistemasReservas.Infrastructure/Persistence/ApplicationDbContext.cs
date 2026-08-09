using Microsoft.EntityFrameworkCore;
using SistemasReservas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Reserva> Reservas { get; set; }
}
