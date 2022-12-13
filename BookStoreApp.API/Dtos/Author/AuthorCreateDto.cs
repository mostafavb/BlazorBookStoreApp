using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Dtos.Author;

public class AuthorCreateDto 
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(250)]
    public string Bio { get; set; } = string.Empty;
}
