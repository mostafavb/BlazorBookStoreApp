
namespace BookStoreApp.Blazor.Server.UI.Services.Authore;

public interface IAuthorService
{
    Task<Response<List<AuthorDto>>> GetAuthors();
    Task<Response<int>> CreateAuthor(AuthorCreateDto author);
    Task<Response<int>> EditAuthor(int id, AuthorDto author);
    Task<Response<AuthorDto>> GetAuthor(int id);
    Task<Response<AuthorDetailsDto>> GetAuthorDetails(int id);
    Task<Response<int>> DeleteAuthor(int id);
}