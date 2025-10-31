using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models;

public class Purchase
{
    [Key]
    public Guid id { get; set; } = Guid.NewGuid();
    
    [Required]
    public DateTime date { get; set; }
    [Required]
    public Guid client { get; set; }
    [Required]
    public Guid vehicle { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(client))]
    public Client? Client { get; set; }

    [ForeignKey(nameof(vehicle))]
    public Vehicle? Vehicle { get; set; }
    
}