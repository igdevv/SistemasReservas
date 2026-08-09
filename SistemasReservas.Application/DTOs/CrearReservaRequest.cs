using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Application.DTOs;

public class CrearReservaRequest
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
