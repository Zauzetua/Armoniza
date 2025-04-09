using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Grupos artisticos
/// </summary>
public partial class grupo
{
    public int id { get; set; }

    [Required(ErrorMessage = "El nombre del grupo es requerido")]
    public string grupo1 { get; set; } = null!;

    /// <summary>
    /// soft delete
    /// </summary>
    public bool eliminado { get; set; }

    public virtual ICollection<grupoDirector> grupoDirector { get; set; } = new List<grupoDirector>();

    public virtual ICollection<usuario> usuario { get; set; } = new List<usuario>();
}
