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
    private readonly ILogger<SocialStatsController> _logger;

    public SocialStatsController(AppDbContext context, ILogger<SocialStatsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET /api/socialstats
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SocialStats>>> GetAll()
    {
        var stats = await _context.SocialStats.AsNoTracking().ToListAsync();
        _logger.LogInformation("Consulta de estadísticas: {Count} registros devueltos", stats.Count);
        return Ok(stats);
    }

    // GET /api/socialstats/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SocialStats>> GetById(int id)
    {
        var stat = await _context.SocialStats.FindAsync(id);
        if (stat is null)
        {
            _logger.LogWarning("Estadística con id {Id} no encontrada", id);
            return NotFound();
        }
        return Ok(stat);
    }

    // POST /api/socialstats
    [HttpPost]
    public async Task<ActionResult<SocialStats>> Create(SocialStats stat)
    {
        _context.SocialStats.Add(stat);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Estadística creada: id {Id}, name {Name}, level {Level}", stat.Id, stat.Name, stat.Level);
        return CreatedAtAction(nameof(GetById), new { id = stat.Id }, stat);
    }

    // PUT /api/socialstats/3
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SocialStats stat)
    {
        if (id != stat.Id)
        {
            _logger.LogWarning("Update fallido: id de URL {UrlId} no coincide con id del body {BodyId}", id, stat.Id);
            return BadRequest("El id de la URL no coincide con el del body");
        }

        var existing = await _context.SocialStats.FindAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Update fallido: estadística con id {Id} no encontrada", id);
            return NotFound();
        }

        existing.Name = stat.Name;
        existing.Level = stat.Level;
        existing.Points = stat.Points;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Estadística actualizada: id {Id}, name {Name}, level {Level}", id, stat.Name, stat.Level);
        return NoContent();
    }

    // DELETE /api/socialstats/3
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var stat = await _context.SocialStats.FindAsync(id);
        if (stat is null)
        {
            _logger.LogWarning("Delete fallido: estadística con id {Id} no encontrada", id);
            return NotFound();
        }

        _context.SocialStats.Remove(stat);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Estadística eliminada: id {Id}, name {Name}", id, stat.Name);
        return NoContent();
    }
}