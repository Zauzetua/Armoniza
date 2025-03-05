using System;
using System.Collections.Generic;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Representa los tipos de usuario (director, instructor, aspirantes y alumno grupo artistico (IGA))
/// </summary>
public partial class tipoUsuario
{
    public int id { get; set; }

    public string tipo { get; set; } = null!;

    public bool eliminado { get; set; }

    /// <summary>
    /// Cuantos instrumentos puede pedir el usuario segun su tipo
    /// </summary>
    public int capacidadInstrumentos { get; set; }

    public virtual ICollection<usuario> usuario { get; set; } = new List<usuario>();
}
