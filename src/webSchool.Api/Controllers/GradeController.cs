namespace webEscuela.Api.Controllers;

using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _svc;

    public GradeController(IGradeService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<ActionResult<List<GradeDto>>> GetAll() =>
        Ok(await _svc.AllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GradeDto>> ById(int id)
    {
        var ent = await _svc.ByIdAsync(id);
        if (ent == null) return NotFound();
        return Ok(ent);
    }

    [HttpPost]
    public async Task<ActionResult<GradeDto>> Create([FromBody] GradeDto dto)
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
    public async Task<IActionResult> Update(int id, [FromBody] GradeDto dto)
    {
        if (dto.Id == null || dto.Id != id)
            return BadRequest("El Id debe coincidir con el cuerpo.");

        try
        {
            var ok = await _svc.UpdateAsync(dto);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
