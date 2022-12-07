
using AutoMapper;
using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Models;
using BookStoreApp.API.Statics;
using Microsoft.AspNetCore.Mvc;


namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        private readonly ILogger<AuthorsController> logger;
        private readonly IMapper mapper;

        public AuthorsController(BookStoreDbContext context, ILogger<AuthorsController> logger, IMapper mapper)
        {
            _context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            try
            {
                if (_context.Authors == null)
                {
                    logger.LogWarning("Entity set for 'BookStoreDbContext.Authors' is null.");
                    return Problem("Entity set for DbContext is null.");
                }
                var authors = await _context.Authors.ToListAsync();
                var authorDtos = mapper.Map<IEnumerable<AuthorDto>>(authors);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(GetAuthors)}");
                return Problem(Messages.Error500, statusCode: 500);
            }

        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Authors' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            try
            {

                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {                    
                    return NotFound("This Id doesn't match any Author!");
                }
                var authorDto = mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(GetAuthor)} id {id}");
                return Problem(Messages.Error500, statusCode: 500);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDto authorDto)
        {
            if (id != authorDto.Id)
            {
                logger.LogWarning($"Record was not matched in {nameof(PutAuthor)} id {id} and {authorDto}");
                return BadRequest("Something is wrong! check your input data.");
            }
            var author = await _context.Authors.FindAsync(id);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AuthorExists(id))
                {
                    logger.LogWarning($"Record was not found in {nameof(PutAuthor)} id {id}");
                    return NotFound("This Id doesn't match any Author!");
                }
                else
                {
                    logger.LogError(ex, $"Error happend in {nameof(PutAuthor)} id {id} and {authorDto}");
                    return Problem(Messages.Error500, statusCode: 500);
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorCreateDto)
        {
            if (_context.Authors == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Authors' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            var author = mapper.Map<Author>(authorCreateDto);

            _context.Authors.Add(author);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(PostAuthor)} author {authorCreateDto}");
                return Problem(Messages.Error500, statusCode: 500);
            }

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {

            if (_context.Authors == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Authors' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    logger.LogWarning($"Record was not found in {nameof(DeleteAuthor)} id {id}");
                    return NotFound("This Id doesn't match any Author!");
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(DeleteAuthor)} id {id}");
                return Problem(Messages.Error500, statusCode: 500);
            }
            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
