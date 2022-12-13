namespace BookStoreApp.Blazor.Server.UI.Services.Books;

public interface IBookService
{
    Task<Response<List<BookDto>>> GetBooksByAuthorId(int authorId);
}
