using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Categoria para instrumentos, viento, percursion, etc
/// </summary>
[Table("categoria")]
public partial class categoria
{
    public string categoria1 { get; set; } = null!;

    public bool eliminado { get; set; }

    public int id { get; set; }

    public virtual ICollection<instrumento> instrumento { get; set; } = new List<instrumento>();
}
