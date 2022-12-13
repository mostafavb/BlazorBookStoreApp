using BookStoreApp.Blazor.Server.UI.Services.Base;
using System.Collections.Generic;

namespace BookStoreApp.Blazor.Server.UI.Services.Books;

public class BookService : BaseHttpService, IBookService
{
    private readonly IClient client;

    public BookService(IClient client) : base(client)
    {
        this.client = client;
    }
    public async Task<Response<List<BookDto>>> GetBooksByAuthorId(int authorId)
    {
        Response<List<BookDto>> response;
        try
        {
            //throw new Exception("Wow there is a fatal error occured in system! Object String is null");

            var data = await client.BooksByAuthorAsync(authorId);
            response = new()
            {
                Data = data.ToList(),
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<List<BookDto>>(ex);
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