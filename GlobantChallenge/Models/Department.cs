using System;
using System.Collections.Generic;

namespace GlobantChallenge.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? Department1 { get; set; }

    public virtual ICollection<HiredEmployee> HiredEmployees { get; set; } = new List<HiredEmployee>();
}
