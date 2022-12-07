using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Dtos.Book;

public class BookCreateDto
{
    [Required]
    [StringLength(50)]
    public string? Title { get; set; } = string.Empty;

    [Required]
    [Range(1900,int.MaxValue)]
    public int? Year { get; set; } = DateTime.Now.Year;

    [Required]    
    public string Isbn { get; set; } = null!;

    [Required]
    [StringLength(50,MinimumLength =10)]
    public string? Summary { get; set; } = string.Empty;

    public string? Image { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public decimal? Price { get; set; } = 0;

}
