using System.ComponentModel.DataAnnotations;
using Domain.Specialists;

namespace Domain.Specializations;
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
