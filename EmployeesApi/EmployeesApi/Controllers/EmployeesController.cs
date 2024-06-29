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
public class EmployeesController : ControllerBase
{
    private readonly ApiDbContext _dbContext;

    public EmployeesController(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _dbContext.Employees.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee([FromForm] Employee employee)
    {
        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }
        _dbContext.Entry(employee).State = EntityState.Modified;
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
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
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    private bool EmployeeExists(int id)
    {
        return _dbContext.Employees.Any(e => e.Id == id);
    }
}
