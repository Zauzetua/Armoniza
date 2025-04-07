using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Categoria para instrumentos, viento, percursion, etc
/// </summary>
[Table("categoria")]
public partial class categoria
{
    [Required(ErrorMessage ="El nombre de la categoria es requerido")]
    [DisplayName("Nombre de la Categoria")]
    public string categoria1 { get; set; } = null!;

    public bool eliminado { get; set; }

    public int id { get; set; }

    public virtual ICollection<instrumento> instrumento { get; set; } = new List<instrumento>();
}
