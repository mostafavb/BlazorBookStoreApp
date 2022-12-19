using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    private readonly BookStoreDbContext db;
    private readonly IMapper mapper;

    public BooksRepository(BookStoreDbContext db, IMapper mapper) : base(db)
    {
        this.db = db;
        this.mapper = mapper;
    }


    public async Task<List<BookDto>> GetBooksWithDetail() =>
        await db.Books
                    .Include(i => i.Author)
                    .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

    public async Task<List<BookDto>> GetBooksWithDetailByAuthorId(int authorId) =>
        await db.Books
                   .Include(i => i.Author)
                   .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                   .Where(w => w.AuthorId == authorId)
                   .ToListAsync();

}