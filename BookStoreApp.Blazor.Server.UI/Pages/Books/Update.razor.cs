using BookStoreApp.Blazor.Server.UI.Services.Author;
using BookStoreApp.Blazor.Server.UI.Services.Book;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Reflection;

namespace BookStoreApp.Blazor.Server.UI.Pages.Books;

public partial class Update
{
    [Parameter] public int Id { get; set; }
    [Inject] IBookService bookService { get; set; }
    [Inject] IAuthorService authorService { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    List<AuthorDto> authors;
    //private BookDto book;
    private BookUpdateDto model;
    Response<BookUpdateDto> response;
    string uploadFileWarning;
    string img;
    double fileSize = 1024 * 1024 * 0.5;
    protected override async Task OnInitializedAsync()
    {
        var authorResponse = await authorService.GetAuthors();
        if (authorResponse.Success)
            authors = authorResponse.Data;
        response = await bookService.GetBookForEdit(Id);
        if (response.Success)
        {
            model = response.Data;
            img = response.Data.Image;
        }
    }

    private async Task HandleEditBook()
    {
        var result = await bookService.EditBook(Id, model);
        if (result.Success)
        {
            Snackbar.Add("The Book saved successfully.", Severity.Success, (config =>
            {
                config.Action = "Return to the list.";
                config.Onclick = snak => { BackToList(); return Task.CompletedTask; };
            }));
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private async Task HandelFileSelection(InputFileChangeEventArgs e)
    {
        ClearCash();
        var file = e.File;
        if (file != null)
        {
            if (file.Size > fileSize)
                uploadFileWarning = $"Please note that maximum size for file to upload is {fileSize/1024}KB.";
            else
            {

                try
                {
                    var ext = System.IO.Path.GetExtension(file.Name);
                    if (ext.ToLower().Contains("jpg") || ext.ToLower().Contains("jpeg") || ext.ToLower().Contains("png"))
                    {
                        var resizedImage = await file.RequestImageFileAsync("image/png", 450, 582);
                        using var stream = resizedImage.OpenReadStream();
                        using var ms = new MemoryStream();
                        await stream.CopyToAsync(ms);
                        string base64String = Convert.ToBase64String(ms.ToArray());
                        string imageType = resizedImage.ContentType;

                        model.ImageData = base64String;
                        model.OriginalImageName = file.Name;

                        img = $"data:{imageType}; base64, {base64String}";

                        //var resizedImage = await file.RequestImageFileAsync("image/png", 450, 582);
                        ////var resizedImage = file;
                        //var buffer = new byte[resizedImage.Size];
                        //await resizedImage.OpenReadStream().ReadAsync(buffer);
                        //string imageType = resizedImage.ContentType;
                        //string base64String = Convert.ToBase64String(buffer);
                        //model.ImageData = base64String;
                        //model.OriginalImageName = resizedImage.Name;
                        //img = $"data:{imageType}; base64, {base64String}";
                    }
                    else
                        uploadFileWarning = "Please select a valid image file (*.jpg | *.png)";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
    private void ClearCash()
    {
        model.OriginalImageName =
            img =
            model.Image =
            model.ImageData =
            uploadFileWarning = string.Empty;
    }
    private void BackToList()
    {
        navigationManager.NavigateTo("/books");
    }
}
