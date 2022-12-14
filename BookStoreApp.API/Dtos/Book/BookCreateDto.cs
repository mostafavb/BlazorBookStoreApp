using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Dtos.Book;

public class BookCreateDto
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Required]
    [Range(1700,int.MaxValue)]
    public int Year { get; set; } = DateTime.Now.Year;

    [Required]    
    public string Isbn { get; set; } 

    [Required]
    [StringLength(500,MinimumLength =10)]
    public string Summary { get; set; } 

    public string ImageData { get; set; }
    public string OriginalImageName { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, int.MaxValue,ErrorMessage ="Select an valid aouthor")]
    public int AuthorId { get; set; } 

}
