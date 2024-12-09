using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        if (user == null)
        {
            return BadRequest("User data required!!!");
        }

        try
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        catch (System.Exception)
        {
            return StatusCode(500, "An error occured while creating new user. Please try again later");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        var result = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        context.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
             return StatusCode(500, "An error occured while creating new user. Please try again later");
        }
        return NoContent();
    }
}