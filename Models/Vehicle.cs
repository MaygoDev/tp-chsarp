using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [NotMapped] public bool csvPurchased { get; set; }
    
    public bool isSold() {
        return this.purchase != null;
    }
}