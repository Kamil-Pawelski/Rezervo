using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Schedule;
public sealed class Schedule
{
    public Guid Id { get; set; }
    public Guid SpecialistId { get; set; }
    public DateTime StarTime { get; set; }
    public DateTime EndTime { get; set; }
    public Status Status { get; set; }
}
