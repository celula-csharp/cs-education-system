namespace docker_sharp.Controllers;

using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _svc;

    public StudentController(IStudentService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAll() =>
        Ok(await _svc.AllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> ById(int id)
    {
        var ent = await _svc.ByIdAsync(id);
        if (ent == null) return NotFound();
        return Ok(ent);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create([FromBody] StudentDto dto)
    {
        var created = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(ById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentDto dto)
    {
        if (dto.Id == null || dto.Id != id) return BadRequest("el Id debe de coincidir con el cuerpo.");
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
