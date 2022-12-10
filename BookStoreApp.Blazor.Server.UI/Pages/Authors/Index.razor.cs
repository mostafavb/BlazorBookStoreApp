using Blazored.Toast.Services;
using BookStoreApp.Blazor.Server.UI.Services.Authore;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BookStoreApp.Blazor.Server.UI.Pages.Authors;

public partial class Index
{
    [Inject] IAuthorService authorService { get; set; }
    [Inject] IToastService toastService { get; set; }
    [Inject] IJSRuntime js { get; set; }
    private Response<List<AuthorDto>> response;//= new Response<List<AuthorDto>>() { Success = false };

    protected override async Task OnInitializedAsync()
    {
        response = await authorService.GetAuthors();
    }

    async Task Delete(int id)
    {
        var author = response.Data.FirstOrDefault(f => f.Id == id);
        if (author != null)
        {
            var confirm = await js.InvokeAsync<bool>("confirm", $"Are you sure you want to 'Delete' {author.FirstName} {author.LastName}?");
            if (confirm)
            {
                var result = await authorService.DeleteAuthor(id);
                if (result.Success)
                {
                    toastService.ShowSuccess($"The Author with id {author.FirstName} {author.LastName} deleted successfully. ", "SUCCESS");
                    await OnInitializedAsync();
                }
                else
                {
                    toastService.ShowError(result.Message, "ERROR");
                }
            }
        }
    }
}
