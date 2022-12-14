using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services.Book;

public interface IBookService
{
    Task<Response<List<BookDto>>> GetBooksByAuthorId(int authorId);
    Task<Response<List<BookDto>>> GetBooks();
    Task<Response<int>> CreateBook(BookCreateDto book);
    Task<Response<int>> EditBook(int id, BookUpdateDto book);
    Task<Response<BookDto>> GetBook(int id);
    Task<Response<int>> DeleteBook(int id);
}
