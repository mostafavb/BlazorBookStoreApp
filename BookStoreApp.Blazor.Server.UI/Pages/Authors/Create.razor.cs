using Blazored.Toast.Services;
using BookStoreApp.Blazor.Server.UI.Services.Authore;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Create
{
    [Inject] IAuthorService authorService { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    //[Inject] IToastService toastService { get; set; }

    private AuthorCreateDto Author;
    private Response<int> result;

    protected override void OnInitialized()
    {
        Author = new();
    }
    private async Task HandleCreateAuthor()
    {
         result = await authorService.CreateAuthor(Author);
        //if (result.Success)
        //{
        //    toastService.ShowSuccess("The Author saved successfully. Return to the list.", "SUCCESS", (() => BackToList()));
        //}
        //else
        //{
        //    toastService.ShowError(result.Message, "ERROR");
        //}
    }

    private void BackToList()
    {
        navigationManager.NavigateTo("/authors");
    }
}
