using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profesional.Application.DTOs;
using Profesional.Application.Services;

namespace Profesional.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var pacientes = await _pacienteService.GetAllAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PacienteCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paciente = await _pacienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PacienteCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paciente = await _pacienteService.UpdateAsync(id, dto);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pacienteService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}