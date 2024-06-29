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
public class RolesController : ControllerBase
{
    private readonly ApiDbContext _dbContext;

    public RolesController(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _dbContext.Roles.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await _dbContext.Roles.FindAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        return role;
    }

    [HttpPost]
    public async Task<ActionResult<Role>> PostRole([FromForm] Role role)
    {
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction("GetRole", new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRole(int id, Role role)
    {
        if (id != role.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(role).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoleExists(id))
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
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _dbContext.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool RoleExists(int id)
    {
        return _dbContext.Roles.Any(r => r.Id == id);
    }
}
