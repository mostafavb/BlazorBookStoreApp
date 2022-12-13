using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Models;
using BookStoreApp.API.Statics;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext context;

        private readonly ILogger<BooksController> logger;
        private readonly IMapper mapper;

        public BooksController(BookStoreDbContext context, ILogger<BooksController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            if (context.Books == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Books' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            try
            {
                var books = await context.Books
                    .Include(i => i.Author)
                    .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
                //var bookDtos = mapper.Map<IEnumerable<BookDto>>(books);
                return Ok(books);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(GetBooks)}");
                return Problem(Messages.Error500, statusCode: 500);
            }
        }

        // GET: api/Books/BooksByAuthor/id
        [HttpGet("BooksByAuthor/{authorId}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByAuthorId(int authorId)
        {
            if (context.Books == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Books' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            try
            {
                var books = await context.Books
                    .Include(i => i.Author)
                    .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                    .Where(w => w.AuthorId == authorId)
                    .ToListAsync();
                //var bookDtos = mapper.Map<IEnumerable<BookDto>>(books);
                return Ok(books);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(GetBooks)}");
                return Problem(Messages.Error500, statusCode: 500);
            }
        }


        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            if (context.Books == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Books' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            try
            {
                var book = await context.Books
                    .Include(i => i.Author)
                    .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(GetBook)}");
                return Problem(Messages.Error500, statusCode: 500);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                logger.LogWarning($"Record was not matched in {nameof(PutBook)} id {id} and {bookDto}");
                return BadRequest("Something is wrong! check your input data.");
            }

            var book = await context.Books.FindAsync(id);

            mapper.Map(bookDto, book);

            context.Entry(book).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BookExists(id))
                {
                    logger.LogWarning($"Record was not found in {nameof(PutBook)} id {id}");
                    return NotFound("This Id doesn't match any Book!");
                }
                else
                {
                    logger.LogError(ex, $"Error happend in {nameof(PutBook)} id {id} and {bookDto}");
                    return Problem(Messages.Error500, statusCode: 500);
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDto>> PostBook(BookCreateDto bookDto)
        {
            if (context.Books == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Books'  is null.");
            }

            var book = mapper.Map<Book>(bookDto);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (context.Books == null)
            {
                return NotFound();
            }
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
