using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using ToDoApp.Api.Data;
using ToDoApp.Api.Models;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        // Helper Method: Extract the authenticated user's ID from claims
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID claim is missing.");
            }
            return int.Parse(userIdClaim);
        }

        // GET: /api/todo
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var userId = GetUserId();
                var todos = await _context.TodoItems
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                return Ok(todos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: /api/todo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            try
            {
                var userId = GetUserId();
                var todo = await _context.TodoItems
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (todo == null)
                {
                    return NotFound("To-Do item not found.");
                }

                return Ok(todo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST: /api/todo
        [HttpPost]
        public async Task<IActionResult> AddTodoItem([FromBody] TodoItem todoItem)
        {
            try
            {
                var userId = GetUserId();
                todoItem.UserId = userId;

                await _context.TodoItems.AddAsync(todoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, todoItem);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT: /api/todo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, [FromBody] TodoItem updatedTodo)
        {
            try
            {
                var userId = GetUserId();

                var existingTodo = await _context.TodoItems
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (existingTodo == null)
                {
                    return NotFound("To-Do item not found.");
                }

                existingTodo.Title = updatedTodo.Title;
                existingTodo.Description = updatedTodo.Description;
                existingTodo.DueDate = updatedTodo.DueDate;
                existingTodo.IsCompleted = updatedTodo.IsCompleted;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE: /api/todo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            try
            {
                var userId = GetUserId();

                var todo = await _context.TodoItems
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (todo == null)
                {
                    return NotFound("To-Do item not found.");
                }

                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
