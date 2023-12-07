using System;
using System.Collections.Generic;

namespace GlobantChallenge.Models;

public partial class Job
{
    public int Id { get; set; }

    public string? Job1 { get; set; }

    public virtual ICollection<HiredEmployee> HiredEmployees { get; set; } = new List<HiredEmployee>();
}
