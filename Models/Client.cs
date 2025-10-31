using System.ComponentModel.DataAnnotations;

namespace CarDealer.Models;

public class Client
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string firstName { get; set; }
    [Required]
    public string lastName { get; set; }
    
    [Required]
    public DateTime birthDate { get; set; }
    
    [Required]
    public string email { get; set; }
    [Phone]
    [Required]
    public string phoneNumber { get; set; }
    
    public List<Vehicle>? vehicles { get; set; }
    
}