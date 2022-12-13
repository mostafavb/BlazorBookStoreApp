global using BookStoreApp.Blazor.Server.UI.Services.Base;
//using Blazored.LocalStorage;

using BookStoreApp.Blazor.Server.UI.Configurations;
using BookStoreApp.Blazor.Server.UI.Services.Authore;
using BookStoreApp.Blazor.Server.UI.Services.Books;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddBlazoredToast();
builder.Services.AddMudServices(config => MudServiceConfiguration.Config());

builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://localhost:7036"));

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
