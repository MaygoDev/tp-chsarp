using System.ComponentModel.DataAnnotations;

namespace CarDealer.Models;

public class Vehicle
{
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string manufacturer { get; set; }
    [Required]
    public string model { get; set; }
    
    [Required]
    public int year { get; set; }
    
    [Required]
    public double priceExclTax { get; set; }
    [Required]
    public double priceInclTax { get; set; }
    
    [Required]
    public string color { get; set; }
    
    public Purchase? purchase { get; set; }

    public bool IsSold => purchase != null;
}