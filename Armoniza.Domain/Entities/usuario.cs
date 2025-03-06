using System;
using System.Collections.Generic;

namespace Armoniza.Infrastructure.Infrastructure.Data;

/// <summary>
/// Tabla de usuarios,  hay 4 tipos de usuarios, aspirantes, alumnos grupo artístico, instructores y directores. Cada uno tiene un idTipoUsuario. El campo de grupoId es opcional para algunos tipos de usuarios, por lo que es nulo. 
/// </summary>
public partial class usuario
{
    public int id { get; set; }

    /// <summary>
    /// Nombre completo, todo minusculas
    /// </summary>
    public string nombreCompleto { get; set; } = null!;

    /// <summary>
    /// opcional
    /// </summary>
    public string? telefono { get; set; }

    /// <summary>
    /// Obligatorio, por este medio se mandaran avisos para que regrese el instrumento
    /// </summary>
    public string correo { get; set; } = null!;

    public int idTipo { get; set; }

    /// <summary>
    /// Opcional, ya que los aspirantes no tienen grupo, asimismo, los directores no están dentro, sino que los dirigen, pero esa relación se hace en la tabla GrupoDirector, del esquema catalogo. 
    /// </summary>
    public int? idGrupo { get; set; }

    public bool eliminado { get; set; }

    public virtual ICollection<apartado> apartados { get; set; } = new List<apartado>();

    public virtual ICollection<detalleUsuario> detalleUsuarioidAspiranteNavigations { get; set; } = new List<detalleUsuario>();

    public virtual ICollection<detalleUsuario> detalleUsuarioidDirectorNavigations { get; set; } = new List<detalleUsuario>();

    public virtual ICollection<grupoDirector> grupoDirectors { get; set; } = new List<grupoDirector>();

    public virtual grupo? idGrupoNavigation { get; set; }

    public virtual tipoUsuario idTipoNavigation { get; set; } = null!;
}
