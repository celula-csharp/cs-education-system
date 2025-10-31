namespace webEscuela.Api.Controllers;

using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InscriptionController : ControllerBase
{
    private readonly IInscriptionService _svc;

    public InscriptionController(IInscriptionService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<ActionResult<List<InscriptionDto>>> GetAll() =>
        Ok(await _svc.AllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InscriptionDto>> ById(int id)
    {
        var ent = await _svc.ByIdAsync(id);
        if (ent == null) return NotFound();
        return Ok(ent);
    }

    [HttpPost]
    public async Task<ActionResult<InscriptionDto>> Create([FromBody] InscriptionDto dto)
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
    public async Task<IActionResult> Update(int id, [FromBody] InscriptionDto dto)
    {
        if (dto.Id == null || dto.Id != id)
            return BadRequest("El Id debe coincidir con el cuerpo.");

        var ok = await _svc.UpdateAsync(dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
