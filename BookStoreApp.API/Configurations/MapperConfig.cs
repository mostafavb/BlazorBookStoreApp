using AutoMapper;
using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Configurations;

public class MapperConfig : Profile
{
	public MapperConfig()
	{
		CreateMap<AuthorCreateDto, Author>().ReverseMap();
		CreateMap<AuthorDto, Author>().ReverseMap();
	}
}
