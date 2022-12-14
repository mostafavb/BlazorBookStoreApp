namespace BookStoreApp.Blazor.Server.UI.Services.Author;

public class AuthorService : BaseHttpService, IAuthorService
{
    private readonly IClient client;

    public AuthorService(IClient client) : base(client)
    {
        this.client = client;
    }

    public async Task<Response<int>> CreateAuthor(AuthorCreateDto author)
    {
        Response<int> response;
        try
        {
            await client.AuthorsPOSTAsync(author);
            response = new()
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        catch (Exception ex)
        {
            response = new()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<List<AuthorDto>>> GetAuthors()
    {
        Response<List<AuthorDto>> response;
        try
        {
            //await Task.Delay(2000);
            var data = await client.AuthorsAllAsync();
            response = new Response<List<AuthorDto>>()
            {
                Data = data.ToList(),
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<List<AuthorDto>>(ex);
        }
        catch (Exception ex)
        {
            response = new Response<List<AuthorDto>>()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<int>> EditAuthor(int id, AuthorDto author)
    {
        Response<int> response;
        try
        {
            await client.AuthorsPUTAsync(id, author);
            response = new()
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        catch (Exception ex)
        {

            response = new()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<AuthorDto>> GetAuthor(int id)
    {
        Response<AuthorDto> response;
        try
        {
            //await Task.Delay(2000);
            //throw new Exception("Its a big error please call your administrator.");
            var data = await client.AuthorsGETAsync(id);
            response = new Response<AuthorDto>()
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<AuthorDto>(ex);
        }
        catch (Exception ex)
        {
            response = new Response<AuthorDto>()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<AuthorDetailsDto>> GetAuthorDetails(int id)
    {
        Response<AuthorDetailsDto> response;
        try
        {
            //await Task.Delay(2000);
            //throw new Exception("Its a big error please call your administrator.");
            var data = await client.AuthorDetailsAsync(id);
            response = new Response<AuthorDetailsDto>()
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<AuthorDetailsDto>(ex);
        }
        catch (Exception ex)
        {
            response = new Response<AuthorDetailsDto>()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<int>> DeleteAuthor(int id)
    {
        Response<int> response;
        try
        {
            await client.AuthorsDELETEAsync(id);
            response = new()
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<int>(ex);
        }
        catch (Exception ex)
        {

            response = new()
            {
                Message = ex.Message,
            };
        }
        return response;
    }
}
