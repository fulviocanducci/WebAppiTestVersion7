using Canducci.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using WebAppiTest.Models;
namespace WebAppiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class TasksController : ControllerBase
    {
        private readonly MyDataBaseContext _context;

        public TasksController(MyDataBaseContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Tasks>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("[controller]/pages/{page?}/{total?}")]
        [ProducesResponseType(typeof(PaginatedRest<Tasks>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedRest<Tasks>>> GetPageTasks(int? page = 1, int? total = 10)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            return await _context.Tasks.OrderBy(o => o.Description).ToPaginatedRestAsync(page ?? 1, total ?? 10);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tasks), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tasks>> GetTasks(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.FindAsync(id);

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Tasks), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutTasks(int id, Tasks tasks)
        {
            if (id != tasks.Id)
            {
                return BadRequest();
            }

            _context.Entry(tasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
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

        [HttpPost]
        [ProducesResponseType(typeof(Tasks), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Tasks), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks tasks)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'MyDataBaseContext.Tasks'  is null.");
            }
            _context.Tasks.Add(tasks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasks", new { id = tasks.Id }, tasks);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Tasks), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Tasks), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTasks(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TasksExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
