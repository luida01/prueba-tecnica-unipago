using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonaStatsApi.Data;
using PersonaStatsApi.Models;

namespace PersonaStatsApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SocialStatsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SocialStatsController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/socialstats
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SocialStats>>> GetAll()
    {
        return Ok(await _context.SocialStats.AsNoTracking().ToListAsync());
    }

    // GET /api/socialstats/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SocialStats>> GetById(int id)
    {
        var stat = await _context.SocialStats.FindAsync(id);
        return stat is null ? NotFound() : Ok(stat);
    }

    // POST /api/socialstats
    [HttpPost]
    public async Task<ActionResult<SocialStats>> Create(SocialStats stat)
    {
        _context.SocialStats.Add(stat);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = stat.Id }, stat);
    }

    // PUT /api/socialstats/3
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SocialStats stat)
    {
        if (id != stat.Id) return BadRequest("El id de la URL no coincide con el del body");

        var existing = await _context.SocialStats.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Name = stat.Name;
        existing.Level = stat.Level;
        existing.Points = stat.Points;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/socialstats/3
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var stat = await _context.SocialStats.FindAsync(id);
        if (stat is null) return NotFound();

        _context.SocialStats.Remove(stat);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}