
namespace BookStoreApp.Blazor.Server.UI.Services.Authore;

public interface IAuthorService
{
    Task<Response<List<AuthorDto>>> GetAuthors();
}