using System;
using System.Collections.Generic;

namespace Armoniza.Infrastructure.Infrastructure.Data;

/// <summary>
/// Tabla de instrumentos, tiene fk con categoría de instrumentos
/// </summary>
public partial class instrumento
{
    /// <summary>
    /// Es el &quot;id&quot; del instrumento, pero debe llamarse codigo por como se manejan los codigos del instrumento en itson centro. No debe der autoincremental, porque los instrumentos ya tienen un codigo
    /// </summary>
    public int codigo { get; set; }

    /// <summary>
    /// Si el instrumento tiene o no estuche
    /// </summary>
    public bool estuche { get; set; }

    /// <summary>
    /// Esta siendo usado o no
    /// </summary>
    public bool ocupado { get; set; }

    /// <summary>
    /// Si el instrumento sirve o no (De no hacerlo, se deberia mandar a arreglar, pero eso sale del alcance)
    /// </summary>
    public bool funcional { get; set; }

    /// <summary>
    /// Soft delete, por si hay algun error de dedo
    /// </summary>
    public bool eliminado { get; set; }

    /// <summary>
    /// Id de la categoria a la que pertenece
    /// </summary>
    public int idCategoria { get; set; }

    public string nombre { get; set; } = null!;

    public virtual ICollection<detalleApartado> detalleApartados { get; set; } = new List<detalleApartado>();

    public virtual categorium idCategoriaNavigation { get; set; } = null!;
}
