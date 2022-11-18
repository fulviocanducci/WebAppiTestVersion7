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
    public class PostsController : ControllerBase
    {
        private readonly MyDataBaseContext _context;

        public PostsController(MyDataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Posts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Posts>>> GetPosts()
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            return await _context.Posts.OrderBy(o => o.Description).ToListAsync();
        }

        [HttpGet("[controller]/pages/{page?}/{total?}")]
        [ProducesResponseType(typeof(PaginatedRest<Posts>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedRest<Posts>>> GetPagePosts(int? page = 1, int? total = 10)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            return await _context.Posts.OrderBy(o => o.Description).ToPaginatedRestAsync(page ?? 1, total ?? 10);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Posts), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<ActionResult<Posts>> GetPosts(Guid id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var posts = await _context.Posts.FindAsync(id);

            if (posts == null)
            {
                return NotFound();
            }

            return posts;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Posts), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutPosts(Guid id, Posts posts)
        {
            if (id != posts.Id)
            {
                return BadRequest();
            }

            _context.Entry(posts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostsExists(id))
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
        [ProducesResponseType(typeof(Posts), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Posts), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Posts>> PostPosts(Posts posts)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'MyDataBaseContext.Posts'  is null.");
            }
            _context.Posts.Add(posts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPosts", new { id = posts.Id }, posts);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Posts), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Posts), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePosts(Guid id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var posts = await _context.Posts.FindAsync(id);
            if (posts == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(posts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostsExists(Guid id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
