using BookStoreApp.Blazor.WebAssembly.UI.Services.Author;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Authors;

public partial class Create
{
    [Inject] IAuthorService authorService { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    private AuthorCreateDto Author;
    private Response<int> result;

    protected override void OnInitialized()
    {
        Author = new();
    }
    private async Task HandleCreateAuthor()
    {
         result = await authorService.CreateAuthor(Author);
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
    }

    private void BackToList()
    {
        navigationManager.NavigateTo("/authors");
    }
}
