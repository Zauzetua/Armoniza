using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Tabla de instrumentos, tiene fk con categoría de instrumentos
/// </summary>
public partial class instrumento
{
    /// <summary>
    /// Es el &quot;id&quot; del instrumento, pero debe llamarse codigo por como se manejan los codigos del instrumento en itson centro. No debe der autoincremental, porque los instrumentos ya tienen un codigo
    /// </summary>
    [Required(ErrorMessage = "El codigo del instrumento es requerido")]
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

    [DisplayName("Instrumento:")]
    [Required(ErrorMessage = "El nombre del instrumento es requerido")]
    public string nombre { get; set; } = null!;

    public virtual ICollection<detalleApartado> detalleApartado { get; set; } = new List<detalleApartado>();

    [DisplayName("Categoria:")]

    public virtual categoria? idCategoriaNavigation { get; set; }

    public void CambiarEstado()
    {
        if (ocupado)
        {
            throw new InvalidOperationException("No se puede cambiar el estado de un instrumento ocupado.");
        }

        if (funcional)
        {
            funcional = false;
        }
        else
        {
            funcional = true;
        }
    }

    public void Prestar()
    {
        if (!ocupado)
        {
            ocupado = true;
        }
        else
        {
            throw new InvalidOperationException("No se puede prestar un instrumento que ya esta ocupado.");
        }

        if (!funcional)
        {
            throw new InvalidOperationException("No se puede prestar un instrumento que no es funcional.");
        }

    }

    public void Devolver()
    {
        if (ocupado)
        {
            ocupado = false;
        }
        else
        {
            throw new InvalidOperationException("No se puede devolver un instrumento que no esta ocupado.");
        }
    }
}
