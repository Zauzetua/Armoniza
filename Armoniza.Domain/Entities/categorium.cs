using System;
using System.Collections.Generic;

namespace Armoniza.Infrastructure.Infrastructure.Data;

/// <summary>
/// Categoria para instrumentos, viento, percursion, etc
/// </summary>
public partial class categorium
{
    public string categoria { get; set; } = null!;

    public bool eliminado { get; set; }

    public int id { get; set; }

    public virtual ICollection<instrumento> instrumentos { get; set; } = new List<instrumento>();
}
