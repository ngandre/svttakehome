namespace SVT.Models;

using System.ComponentModel.DataAnnotations;

public class ClosestInputModel {
    [Required]
    public int LoadId { get; set; }

    [Required]
    public double? X { get; set; }

    [Required]
    public double? Y { get; set; }
}