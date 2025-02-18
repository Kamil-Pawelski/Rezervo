using System.ComponentModel.DataAnnotations;

namespace Domain.Specialists;
public sealed class Specialization
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string Name { get; set; }

    public ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
}
