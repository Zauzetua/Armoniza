using System;
using System.Collections.Generic;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Tabla que relaciona a un grupo con su respectivo director, un director puede tener mas de un grupo (No ha pasado, pero es posible)
/// Falta la logica para verificar si el usuario es un director, o se hara en el backend (probablemente ambos)
/// </summary>
public partial class grupoDirector
{
    public int id { get; set; }

    /// <summary>
    /// id del grupo artistico, se relacionara con su director
    /// </summary>
    public int idGrupo { get; set; }

    /// <summary>
    /// id del director al que se le relacionara con un grupo, un director puede tener mas de un grupo (Pero es muy raro, y no ha ocurrido hasta ahora)
    /// </summary>
    public int idDirector { get; set; }

    public virtual usuario idDirectorNavigation { get; set; } = null!;

    public virtual grupo idGrupoNavigation { get; set; } = null!;
}
