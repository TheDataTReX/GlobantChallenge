using System;
using System.Collections.Generic;

namespace GlobantChallenge.Models;

public partial class HiredEmployee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? HireDate { get; set; } // Cambiado de Datetime a DateTime y renombrado a HireDate
    public int? DepartmentId { get; set; }
    public int? JobId { get; set; }
    public virtual Department? Department { get; set; }
    public virtual Job? Job { get; set; }
}

