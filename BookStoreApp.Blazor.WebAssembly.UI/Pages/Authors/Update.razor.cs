using BookStoreApp.Blazor.WebAssembly.UI.Services.Author;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Authors;

public partial class Update
{
    [Parameter] public int Id { get; set; }
    [Inject] IAuthorService authorService { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    private AuthorDto Author;
    Response<AuthorDto> response;

    protected override async Task OnInitializedAsync()
    {
        response = await authorService.GetAuthor(Id);
        if (response.Success)
            Author = response.Data;
    }

    private async Task HandleEditAuthor()
    {
        var result = await authorService.EditAuthor(Id, Author);
        if (result.Success)
        {
            Snackbar.Add("The Author saved successfully.", Severity.Success, config =>
            {
                config.Action = "Return to the list";
                config.ActionColor = Color.Transparent;
                config.Onclick = snackbar =>
                {
                    BackToList();
                    return Task.CompletedTask;
                };
            });
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private void BackToList()
    {
        navigationManager.NavigateTo("/authors");
    }
}
