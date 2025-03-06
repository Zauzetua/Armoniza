using System;
using System.Collections.Generic;

namespace Armoniza.Infrastructure.Infrastructure.Data;

/// <summary>
/// Tabla para relacionar a los aspirantes con su responsable (director).
/// La logica para saber si es o no aspirante y director aun no esta o se hara en el backend (probablemente ambos)
/// </summary>
public partial class detalleUsuario
{
    public int id { get; set; }

    public int idDirector { get; set; }

    public int idAspirante { get; set; }

    public virtual usuario idAspiranteNavigation { get; set; } = null!;

    public virtual usuario idDirectorNavigation { get; set; } = null!;
}
