using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Models;
using BookStoreApp.API.Statics;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        private readonly IWebHostEnvironment webHost;

        public BooksController(BookStoreDbContext context, ILogger<BooksController> logger, IMapper mapper, IWebHostEnvironment webHost)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
            this.webHost = webHost;
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


        // GET: api/Books/BookForEdit/5
        [HttpGet("BookForEdit/{id}")]
        public async Task<ActionResult<BookUpdateDto>> GetBookForEdit(int id)
        {
            if (context.Books == null)
            {
                logger.LogWarning("Entity set for 'BookStoreDbContext.Books' is null.");
                return Problem("Entity set for DbContext is null.");
            }
            try
            {
                var book = await context.Books
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (book == null)
                {
                    return NotFound();
                }
                var bookToUpdate = mapper.Map<BookUpdateDto>(book);
                return Ok(bookToUpdate);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(GetBook)}");
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

            if (!string.IsNullOrEmpty(bookDto.ImageData))
                bookDto.Image = CreateImage(bookDto.ImageData, bookDto.OriginalImageName);

            if (!string.IsNullOrEmpty(book.Image) && book.Image.ToLower() != bookDto.Image.ToLower())
                DeleteImage(book.Image);
            
                
            
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

        private void DeleteImage(string bookImagePath)
        {
            try
            {
                var imageName = Path.GetFileName(bookImagePath);
                var path = $"{webHost.WebRootPath}\\Files\\Images\\Books\\{imageName}";
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"Error occurd when file {bookImagePath} was deleting");
            }            
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
            book.Image = CreateImage(bookDto.ImageData, bookDto.OriginalImageName);
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

        private string CreateImage(string imagebase64, string imageName)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagebase64))
                {
                    var url = HttpContext.Request.Host.Value;
                    var ext = Path.GetExtension(imageName);
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var path = $"{webHost.WebRootPath}\\Files\\Images\\Books\\{fileName}";

                    byte[] image = Convert.FromBase64String(imagebase64);

                    var fileStream = System.IO.File.Create(path);
                    fileStream.Write(image, 0, image.Length);
                    fileStream.Close();

                    return $"https://{url}/Files/Images/Books/{fileName}";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occured in creating image file for {imageName}.");
            }
            return string.Empty;
        }
    }
}
