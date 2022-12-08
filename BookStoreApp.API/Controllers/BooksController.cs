﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using AutoMapper;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Statics;
using AutoMapper.QueryableExtensions;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
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