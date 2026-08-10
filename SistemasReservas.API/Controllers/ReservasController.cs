using Microsoft.AspNetCore.Mvc;
using SistemasReservas.Application.DTOs;
using SistemasReservas.Application.Services;

namespace SistemasReservas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController: ControllerBase
    {
        private readonly ReservaService _reservaService;

        public ReservasController(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public async Task<ActionResult<ReservaResponse>> Get()
        {
            var reservas = await _reservaService.ObtenerTodasAsync();

            return Ok(reservas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservaResponse>> GetPorId(int id)
        {
            var reservas = await _reservaService.ObtenerPorIdAsync(id);

            if(reservas == null)
            {
                return NotFound();
            }

            return Ok(reservas);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearReservaRequest request)
        {
            var reserva = await _reservaService.CrearReservaAsync(request);

            return Ok(reserva);
        }

        [HttpPatch("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(int id)
        {
            await _reservaService.ConfirmarAsync(id);

            return NoContent();
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            await _reservaService.CancelarAsync(id);

            return NoContent();
        }

    }
}
