using BookStoreApp.Blazor.WebAssembly.UI.Services.Author;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Book;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Books;

public partial class Create
{
    public CultureInfo _en = CultureInfo.GetCultureInfo("en-US");
    BookCreateDto model;
    private Response<int> result;
    [Inject] IBookService bookService { get; set; }
    [Inject] IAuthorService authorService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
    List<AuthorDto> authors;
    private string img = string.Empty;
    string uploadFileWarning;
    double fileSize = 1024 * 1024 * 1;
    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("The create new book start");
        model = new() { AuthorId = -1 };
        var response = await authorService.GetAuthors();
        if (response.Success)
            authors = response.Data.ToList();

    }
    private async Task HandelCreatingBook()
    {
        result = await bookService.CreateBook(model);
        if (result.Success)
        {
            Snackbar.Add("The Book saved successfully.", Severity.Success, config =>
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

    private async Task HandelFileSelection(InputFileChangeEventArgs e)
    {
        ClearCash();
        var file = e.File;
        if (file != null)
        {

            if (file.Size > fileSize)
                uploadFileWarning = $"Please note that maximum size for file to upload is {fileSize / 1024}KB.";
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
            model.ImageData =
            uploadFileWarning = string.Empty;
    }

    void BackToList()
    {
        navigationManager.NavigateTo("/books");
    }
}
