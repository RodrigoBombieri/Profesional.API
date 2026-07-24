using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profesional.Application.DTOs;
using Profesional.Application.Services;

namespace Profesional.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SesionesController : ControllerBase
    {
        private readonly ISesionService _sesionService;

        public SesionesController(ISesionService sesionService)
        {
            _sesionService = sesionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sesiones = await _sesionService.GetAllAsync();
            return Ok(sesiones);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPaciente(int pacienteId)
        {
            var sesiones = await _sesionService.GetByPacienteIdAsync(pacienteId);
            return Ok(sesiones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sesion = await _sesionService.GetByIdAsync(id);
            if (sesion == null)
                return NotFound();

            return Ok(sesion);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SesionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var sesion = await _sesionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = sesion.Id }, sesion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SesionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var sesion = await _sesionService.UpdateAsync(id, dto);
                if (sesion == null)
                    return NotFound();

                return Ok(sesion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sesionService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/completar")]
        public async Task<IActionResult> CompletarSesion(int id)
        {
            var result = await _sesionService.CompletarSesionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}