using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Repositories;

public interface IBooksRepository : IGenericRepository<Book>
{
    Task<List<BookDto>> GetBooksWithDetail();
    Task<List<BookDto>> GetBooksWithDetailByAuthorId(int authorId);
}
