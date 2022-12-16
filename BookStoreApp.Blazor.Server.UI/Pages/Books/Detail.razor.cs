using BookStoreApp.Blazor.Server.UI.Services.Book;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.UI.Pages.Books;

public partial class Detail
{
    [Parameter] public int Id { get; set; }

    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] IBookService bookService { get; set; }

    Response<BookDto> response;
    private BookDto book;
    protected override async Task OnInitializedAsync()
    {
        //response = await authorService.GetAuthor(Id);
        response = await bookService.GetBook(Id);
        if (response.Success)
        {
            book = response.Data;
            //Author.FirstName = $"{response.Data.FirstName} {response.Data.LastName}";
        }
    }

    private void BackToList()
    {
        navigationManager.NavigateTo("/books");
    }
    private void GotToEdit()
    {
        navigationManager.NavigateTo($"/books/edit/{book.Id}");
    }
}
