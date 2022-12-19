using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Repositories;

public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository
{
    private readonly BookStoreDbContext db;
    private readonly IMapper mapper;

    public AuthorsRepository(BookStoreDbContext db, IMapper mapper) : base(db)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id) =>
        await db.Authors
                .Include(i => i.Books)
                .ProjectTo<AuthorDetailsDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(f => f.Id == id);

}
