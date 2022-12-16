using BookStoreApp.Blazor.WebAssembly.UI.Services.Book;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Books;

public partial class Index
{
    [Inject] IBookService bookService { get; set; }

    [Inject] private IDialogService DialogService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private Response<List<BookDto>> response;
    [Inject] NavigationManager navigationManager { get; set; }
    private string searchString = "";
    BookDto book = new();

    protected override async Task OnInitializedAsync()
    {
        response = await bookService.GetBooks();
    }
    private bool Search(BookDto book)
    {
        if (string.IsNullOrWhiteSpace(searchString)) return true;
        if (book.AuthorName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || book.Summary.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || book.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || book.Price.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    async Task Delete(int id)
    {
        var _book = response.Data.FirstOrDefault(f => f.Id == id);
        if (_book != null)
        {
            var confirm = await DialogService.ShowMessageBox("Warning",
                                                                         (MarkupString)$"Are you sure you want to <u>Delete</u><b> {_book.Title} </b>?",
                                                                         yesText: "Delete!",
                                                                         cancelText: "Cancel");
            if (confirm ?? false)
            {
                var result = await bookService.DeleteBook(id);
                if (result.Success)
                {

                    Snackbar.Add($"The Book with title {_book.Title} {_book.AuthorName} deleted successfully.", Severity.Success);
                    await OnInitializedAsync();
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                }
            }
        }

    }
}
