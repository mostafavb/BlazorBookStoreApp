﻿namespace BookStoreApp.API.Dtos.Book;

public class BookDto : BaseDto
{
    public string? Title { get; set; }   

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public string Summary { get; set; }

    public int? AuthorId { get; set; }
    public string? AuthorName { get; set;}
}
