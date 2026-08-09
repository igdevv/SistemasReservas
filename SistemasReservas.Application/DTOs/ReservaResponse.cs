using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Application.DTOs;

public class ReservaResponse
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = string.Empty;
}
