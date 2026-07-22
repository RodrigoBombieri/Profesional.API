using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profesional.Application.Services;
using Profesional.Application.DTOs;

namespace Profesional.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ISesionService _sesionService;

        public SesionController(ISesionService sesionService)
        {
            _sesionService = sesionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sesiones = await _sesionService.GetAllAsync();
            return Ok(sesiones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sesion = await _sesionService.GetByIdAsync(id);
            if (sesion == null) return NotFound();
            return Ok(sesion);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SesionCreateDto dto)
        {
            var sesion = await _sesionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = sesion.IdPaciente }, sesion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SesionCreateDto dto)
        {
            var sesion = await _sesionService.UpdateAsync(id, dto);
            if (sesion == null) return NotFound();
            return Ok(sesion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sesionService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

    }
}
