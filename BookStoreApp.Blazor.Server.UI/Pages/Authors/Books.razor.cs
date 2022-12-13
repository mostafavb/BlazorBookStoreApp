using BookStoreApp.Blazor.Server.UI.Services.Books;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Books
{
    [Parameter] public int AuthorId { get; set; }

    [Inject] IBookService bookService{ get; set; }

    private Response<List<BookDto>> response;

    protected override async Task OnInitializedAsync()
    {
        response = await bookService.GetBooksByAuthorId(AuthorId);
    }
}
