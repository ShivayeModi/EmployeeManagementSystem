using EmployeesApi.Data;
using EmployeesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApiDbContext _dbContext;

    public UsersController(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Authorize] // Protects this endpoint
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    [Authorize] // Protects this endpoint
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    [AllowAnonymous] // Allows unauthenticated access for registration
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    [Authorize] // Protects this endpoint
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(user).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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
    [Authorize] // Protects this endpoint
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _dbContext.Users.Any(u => u.Id == id);
    }
}
