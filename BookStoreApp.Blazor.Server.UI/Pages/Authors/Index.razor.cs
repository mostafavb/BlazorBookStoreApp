
using BookStoreApp.Blazor.Server.UI.Services.Authore;
using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Index
{
    [Inject] IAuthorService authorService { get; set; }

    [Inject] private IDialogService DialogService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private Response<List<AuthorDto>> response;//= new Response<List<AuthorDto>>() { Success = false };
    [Inject] NavigationManager navigationManager { get; set; }
    private string searchString = "";
    AuthorDto author = new AuthorDto();
    protected override async Task OnInitializedAsync()
    {
        response = await authorService.GetAuthors();
    }

    async Task Delete(int id)
    {
        var author = response.Data.FirstOrDefault(f => f.Id == id);
        if (author != null)
        {

            var confirm = await DialogService.ShowMessageBox("Warning",
                                                             (MarkupString)$"Are you sure you want to <u>Delete</u><b> {author.FirstName} {author.LastName}</b>?",
                                                             yesText: "Delete!",
                                                             cancelText: "Cancel");

            //var confirm = await js.InvokeAsync<bool>("confirm", $"Are you sure you want to 'Delete' {author.FirstName} {author.LastName}?");
            if (confirm ?? false)
            {
                var result = await authorService.DeleteAuthor(id);
                if (result.Success)
                {

                    //toastService.ShowSuccess($"The Author with id {author.FirstName} {author.LastName} deleted successfully. ", "SUCCESS");
                    Snackbar.Add($"The Author with id {author.FirstName} {author.LastName} deleted successfully.", Severity.Success);
                    await OnInitializedAsync();
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                    //toastService.ShowError(result.Message, "ERROR");
                }
            }
        }
    }

    private bool Search(AuthorDto author)
    {
        if (string.IsNullOrWhiteSpace(searchString)) return true;
        if (author.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || author.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }
}
