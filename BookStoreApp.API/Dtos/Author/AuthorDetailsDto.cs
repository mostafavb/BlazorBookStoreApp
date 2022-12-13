using BookStoreApp.API.Dtos.Book;

namespace BookStoreApp.API.Dtos.Author;

public class AuthorDetailsDto : AuthorDto
{
    public List<BookDto> Books { get; set; }
}
