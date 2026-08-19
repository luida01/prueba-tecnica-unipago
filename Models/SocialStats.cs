using System.ComponentModel.DataAnnotations;

namespace PersonaStatsApi.Models;

public class SocialStats
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty; 

    [Range(1, 5)]
    public int Level { get ; set; } = 1;

    [Range(0, 100)]
    public int Points { get ; set; } = 0;

    
}