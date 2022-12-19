using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Repositories;

public interface IAuthorsRepository : IGenericRepository<Author>
{
    Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id);
}
