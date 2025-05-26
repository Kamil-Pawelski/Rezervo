using System.ComponentModel.DataAnnotations;
using Domain.Specialists;

namespace Domain.Specializations;
public sealed class Specialization
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Specialist> Specialists { get; set; } = [];
}
