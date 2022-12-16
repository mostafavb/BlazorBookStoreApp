using BookStoreApp.Blazor.WebAssembly.UI.Services.Book;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Books;

public partial class Detail
{
    [Parameter] public int Id { get; set; }

    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] IBookService bookService { get; set; }

    Response<BookDto> response;
    private BookDto book;
    protected override async Task OnInitializedAsync()
    {
        response = await bookService.GetBook(Id);
        if (response.Success)
        {
            book = response.Data;
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
