using Blazored.LocalStorage;

namespace BookStoreApp.Blazor.Server.UI.Services.Authore;

public class AuthorService : BaseHttpService, IAuthorService
{
    private readonly IClient client;

    public AuthorService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
    {
        this.client = client;
    }

    public async Task<Response<List<AuthorDto>>> GetAuthors()
    {
        Response<List<AuthorDto>> response;
        try
        {
            var data = await client.AuthorsAllAsync();
            response = new Response<List<AuthorDto>>()
            {
                Data = data.ToList(),
                Success= true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<List<AuthorDto>>(ex);
        }
        catch(Exception ex) 
        {
            response = new Response<List<AuthorDto>>() 
            {
                Message=ex.Message,
            };
        }
        return response;
    }
}
