using System.ComponentModel.DataAnnotations;

namespace Domain.Specialists;
public sealed class Specialization
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(32)]
    public required string Name { get; set; }

    public ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
}


public static class SpecializationNames
{
    public const string Plumber = "Plumber";
    public const string Hairdresser = "Hairdresser";
    public const string Electrician = "Electrician";
    public const string Carpenter = "Carpenter";
    public const string Mechanic = "Mechanic";
    public const string Painter = "Painter";
    public const string Masseur = "Masseur";
}
