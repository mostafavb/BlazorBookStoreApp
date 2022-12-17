global using  Microsoft.EntityFrameworkCore;
global using BookStoreApp.API.Data;

using Serilog;
using BookStoreApp.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BookStoreDbContext>();
builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx,lc)=>
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration)
);


var corsPolicyName = "AllowAll";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: corsPolicyName,
    policy =>
    {
        policy
        //.WithOrigins("https://localhost:44398", "http://localhost:65283/")            
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors(corsPolicyName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
