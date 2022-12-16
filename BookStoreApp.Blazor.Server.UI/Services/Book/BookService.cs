using BookStoreApp.Blazor.Server.UI.Services.Base;
using System.Collections.Generic;

namespace BookStoreApp.Blazor.Server.UI.Services.Book;

public class BookService : BaseHttpService, IBookService
{
    private readonly IClient client;

    public BookService(IClient client) : base(client)
    {
        this.client = client;
    }

    public async Task<Response<int>> CreateBook(BookCreateDto book)
    {
        Response<int> response;
        try
        {
            await client.BooksPOSTAsync(book);
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

    public async Task<Response<int>> DeleteBook(int id)
    {
        Response<int> response;
        try
        {
            await client.BooksDELETEAsync(id);
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

    public async Task<Response<int>> EditBook(int id, BookUpdateDto book)
    {
        Response<int> response;
        try
        {
            await client.BooksPUTAsync(id, book);
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

    public async Task<Response<BookDto>> GetBook(int id)
    {
        Response<BookDto> response;
        try
        {
            //await Task.Delay(2000);
            //throw new Exception("Its a big error please call your administrator.");
            var data = await client.BooksGETAsync(id);
            response = new Response<BookDto>()
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<BookDto>(ex);
        }
        catch (Exception ex)
        {
            response = new Response<BookDto>()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<BookUpdateDto>> GetBookForEdit(int id)
    {
        Response<BookUpdateDto> response;
        try
        {
            //await Task.Delay(2000);
            //throw new Exception("Its a big error please call your administrator.");
            var data = await client.BookForEditAsync(id);
            response = new Response<BookUpdateDto>()
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiException<BookUpdateDto>(ex);
        }
        catch (Exception ex)
        {
            response = new Response<BookUpdateDto>()
            {
                Message = ex.Message,
            };
        }
        return response;
    }

    public async Task<Response<List<BookDto>>> GetBooks()
    {
        Response<List<BookDto>> response;
        try
        {
            //await Task.Delay(2000);
            var data = await client.BooksAllAsync();
            response = new Response<List<BookDto>>()
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
            response = new Response<List<BookDto>>()
            {
                Message = ex.Message,
            };
        }
        return response;
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