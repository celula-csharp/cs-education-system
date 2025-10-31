namespace docker_sharp.Controllers;

using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorService _svc;

    public ProfessorController(IProfessorService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<List<ProfessorDto>>> GetAll() =>
        Ok(await _svc.AllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfessorDto>> ById(int id)
    {
        var ent = await _svc.ByIdAsync(id);
        if (ent == null) return NotFound();
        return Ok(ent);
    }

    [HttpPost]
    public async Task<ActionResult<ProfessorDto>> Create([FromBody] ProfessorDto dto)
    {
        try
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(ById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProfessorDto dto)
    {
        if (dto.Id == null || dto.Id != id) return BadRequest("El Id debe coincidir con el cuerpo.");
        var ok = await _svc.UpdateAsync(dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        if (!ok) return BadRequest("No se puede eliminar el profesor (posible relación con cursos o no existe).");
        return NoContent();
    }
}
