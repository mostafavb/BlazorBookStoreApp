
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Models;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Statics;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers;

[Route("api/[controller]")]
[ApiConventionType(typeof(DefaultApiConventions))]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorsRepository authorsRepository;

    private readonly ILogger<AuthorsController> logger;
    private readonly IMapper mapper;

    public AuthorsController(IAuthorsRepository authorsRepository, ILogger<AuthorsController> logger, IMapper mapper)
    {
        this.authorsRepository = authorsRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        try
        {
            var authors = await authorsRepository.GetAllAsync();
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
        try
        {
            var author = await authorsRepository.GetAsync(id);

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

    // GET: api/Authors/authordetail/5
    [HttpGet("AuthorDetails/{id}")]
    public async Task<ActionResult<AuthorDetailsDto>> GetAuthorDetail(int id)
    {
        try
        {
            var author = await authorsRepository.GetAuthorDetailsAsync(id);

            if (author == null)
            {
                return NotFound("This Id doesn't match any Author!");
            }

            return Ok(author);
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
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAuthor(int id, AuthorDto authorDto)
    {
        if (id != authorDto.Id)
        {
            logger.LogWarning($"Record was not matched in {nameof(PutAuthor)} id {id} and {authorDto}");
            return BadRequest("Something is wrong! check your input data.");
        }
        var author = await authorsRepository.GetAsync(id);
        if (author == null)
        {
            logger.LogWarning($"Record was not found in {nameof(PutAuthor)} id {id}");
            return NotFound("This Id doesn't match any Author!");
        }

        mapper.Map(authorDto, author);

        try
        {
            await authorsRepository.UpdateAsync(author);
        }
        catch (Exception ex)
        {

            logger.LogError(ex, $"Error happend in {nameof(PutAuthor)} id {id} and {authorDto}");
            return Problem(Messages.Error500, statusCode: 500);

        }

        return NoContent();
    }

    // POST: api/Authors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorCreateDto)
    {
        var author = mapper.Map<Author>(authorCreateDto);

        try
        {
            await authorsRepository.AddAsync(author);

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
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {

            if (!await authorsRepository.Exists(id))
            {
                logger.LogWarning($"Record was not found in {nameof(DeleteAuthor)} id {id}");
                return NotFound("This Id doesn't match any Author!");
            }

            await authorsRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error happend in {nameof(DeleteAuthor)} id {id}");
            return Problem(Messages.Error500, statusCode: 500);
        }
        return NoContent();
    }

}
