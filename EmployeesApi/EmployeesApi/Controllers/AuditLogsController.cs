using EmployeesApi.Data;
using EmployeesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly ApiDbContext _dbContext;

    public AuditLogsController(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
    {
        return await _dbContext.AuditLogs.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLog>> GetAuditLog(int id)
    {
        var auditLog = await _dbContext.AuditLogs.FindAsync(id);

        if (auditLog == null)
        {
            return NotFound();
        }

        return auditLog;
    }

    [HttpPost]
    public async Task<ActionResult<AuditLog>> PostAuditLog(AuditLog auditLog)
    {
        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction("GetAuditLog", new { id = auditLog.Id }, auditLog);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuditLog(int id, AuditLog auditLog)
    {
        if (id != auditLog.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(auditLog).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuditLogExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuditLog(int id)
    {
        var auditLog = await _dbContext.AuditLogs.FindAsync(id);
        if (auditLog == null)
        {
            return NotFound();
        }

        _dbContext.AuditLogs.Remove(auditLog);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool AuditLogExists(int id)
    {
        return _dbContext.AuditLogs.Any(a => a.Id == id);
    }
}
