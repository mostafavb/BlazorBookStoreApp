using Blazored.Toast.Services;
using BookStoreApp.Blazor.Server.UI.Services.Authore;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Update
{
    [Parameter] public int Id { get; set; }
    [Inject] IAuthorService authorService { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] IToastService toastService { get; set; }

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
            toastService.ShowSuccess("The Author saved successfully. Click to return to the list.", "SUCCESS", (() => BackToList()));
        }
        else
        {
            toastService.ShowError(result.Message, "ERROR");
        }
    }

    private void BackToList()
    {
        navigationManager.NavigateTo("/authors");
    }
}
