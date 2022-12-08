using BookStoreApp.Blazor.Server.UI.Services.Authore;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Index
{
    [Inject] IAuthorService authorService { get; set; }
    private Response<List<AuthorDto>> response;//= new Response<List<AuthorDto>>() { Success = false };

    protected override async Task OnInitializedAsync()
    {
        response = await authorService.GetAuthors();
    }
}
