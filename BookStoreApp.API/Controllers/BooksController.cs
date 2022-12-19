using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Models;
using BookStoreApp.API.Repositories;
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
        private readonly IBooksRepository bookRepository;

        private readonly ILogger<BooksController> logger;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHost;

        public BooksController(IBooksRepository bookRepository, ILogger<BooksController> logger, IMapper mapper, IWebHostEnvironment webHost)
        {
            this.bookRepository = bookRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.webHost = webHost;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            try
            {
                var books = await bookRepository.GetBooksWithDetail();
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
            try
            {
                var books = await bookRepository.GetBooksWithDetailByAuthorId(authorId);
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
            try
            {
                var book = await bookRepository.GetAsync(id);

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
            try
            {
                var book = await bookRepository.GetAsync(id);

                if (book == null)
                {
                    return NotFound();
                }
                var bookDto = mapper.Map<BookDto>(book);
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

            var book = await bookRepository.GetAsync(id);
            if (book == null)
            {
                logger.LogWarning($"Record was not found in {nameof(PutBook)} id {id}");
                return NotFound("This Id doesn't match any Book!");
            }

            if (!string.IsNullOrEmpty(bookDto.ImageData))
                bookDto.Image = CreateImage(bookDto.ImageData, bookDto.OriginalImageName);

            if (!string.IsNullOrEmpty(book.Image) && book.Image.ToLower() != bookDto.Image.ToLower())
                DeleteImage(book.Image);


            mapper.Map(bookDto, book);

            try
            {
                await bookRepository.UpdateAsync(book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(PutBook)} id {id} and {bookDto}");
                return Problem(Messages.Error500, statusCode: 500);

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
            var book = mapper.Map<Book>(bookDto);
            if (!string.IsNullOrEmpty(bookDto.ImageData))
                book.Image = CreateImage(bookDto.ImageData, bookDto.OriginalImageName);
            try
            {
                await bookRepository.AddAsync(book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(PostBook)}: {bookDto}");
                return Problem(Messages.Error500, statusCode: 500);
            }
            
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
            if (! await bookRepository.Exists(id))
            {
                return NotFound();
            }
            try
            {
            await bookRepository.DeleteAsync(id);            
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error happend in {nameof(DeleteBook)} id: {id} ");
                return Problem(Messages.Error500, statusCode: 500);
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
