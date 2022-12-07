using AutoMapper;
using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<AuthorCreateDto, Author>().ReverseMap();
        CreateMap<AuthorDto, Author>().ReverseMap();

        CreateMap<BookCreateDto, Book>().ReverseMap();
        CreateMap<BookUpdateDto, Book>().ReverseMap();
        CreateMap<Book, BookDto>()
            .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();
    }
}
