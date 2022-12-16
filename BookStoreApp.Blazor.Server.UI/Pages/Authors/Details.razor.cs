using BookStoreApp.Blazor.Server.UI.Services.Author;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Details
{
    [Parameter] public int Id { get; set; }
    [Inject] IAuthorService authorService { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }

    private AuthorDetailsDto Author;
    //Response<AuthorDto> response;
    Response<AuthorDetailsDto> response;

    protected override async Task OnInitializedAsync()
    {
        //response = await authorService.GetAuthor(Id);
        response = await authorService.GetAuthorDetails(Id);
        if (response.Success)
        {
            Author = response.Data;
            Author.FirstName = $"{response.Data.FirstName} {response.Data.LastName}";
        }
    }

    private void BackToList()
    {
        navigationManager.NavigateTo("/authors");
    }
    private void GotToEdit()
    {
        navigationManager.NavigateTo($"/authors/edit/{Author.Id}");
    }
}
