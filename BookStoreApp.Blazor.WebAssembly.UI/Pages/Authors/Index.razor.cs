using BookStoreApp.Blazor.WebAssembly.UI.Services.Author;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Authors;

public partial class Index
{
    [Inject] IAuthorService authorService { get; set; }

    [Inject] private IDialogService DialogService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private Response<List<AuthorDto>> response;//= new Response<List<AuthorDto>>() { Success = false };
    [Inject] NavigationManager navigationManager { get; set; }
    private string searchString = "";
    AuthorDto author = new();
    protected override async Task OnInitializedAsync()
    {
        response = await authorService.GetAuthors();
    }

    async Task Delete(int id)
    {
        var _author = response.Data.FirstOrDefault(f => f.Id == id);
        if (_author != null)
        {

            var confirm = await DialogService.ShowMessageBox("Warning",
                                                             (MarkupString)$"Are you sure you want to <u>Delete</u><b> {_author.FirstName} {_author.LastName}</b>?",
                                                             yesText: "Delete!",
                                                             cancelText: "Cancel");

            //var confirm = await js.InvokeAsync<bool>("confirm", $"Are you sure you want to 'Delete' {author.FirstName} {author.LastName}?");
            if (confirm ?? false)
            {
                var result = await authorService.DeleteAuthor(id);
                if (result.Success)
                {

                    //toastService.ShowSuccess($"The Author with id {author.FirstName} {author.LastName} deleted successfully. ", "SUCCESS");
                    Snackbar.Add($"The Author with id {_author.FirstName} {_author.LastName} deleted successfully.", Severity.Success);
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
