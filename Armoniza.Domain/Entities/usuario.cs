﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Armoniza.Domain.Entities;

/// <summary>
/// Tabla de usuarios,  hay 4 tipos de usuarios, aspirantes, alumnos grupo artístico, instructores y directores. Cada uno tiene un idTipoUsuario. El campo de grupoId es opcional para algunos tipos de usuarios, por lo que es nulo. 
/// </summary>
public partial class usuario
{
    public int id { get; set; }

    /// <summary>
    /// Nombre completo, todo minusculas
    /// </summary>
    [Required(ErrorMessage = "El Nombre Completo es obligatorio.")]
    [DisplayName("Nombre:")]
    public string nombreCompleto { get; set; } = null!;

    /// <summary>
    /// opcional
    /// </summary>
    [DisplayName("Telefono:")]
    public string? telefono { get; set; }

    /// <summary>
    /// Obligatorio, por este medio se mandaran avisos para que regrese el instrumento
    /// </summary>
    [Required(ErrorMessage = "El Correo es obligatorio.")]
    [DisplayName("Correo electronico:")]
    public string correo { get; set; } = null!;

   
    public int idTipo { get; set; }

    /// <summary>
    /// Opcional, ya que los aspirantes no tienen grupo, asimismo, los directores no están dentro, sino que los dirigen, pero esa relación se hace en la tabla GrupoDirector, del esquema catalogo. 
    /// </summary>

    [DisplayName("Grupo artistico:")]
    public int? idGrupo { get; set; }

    public bool eliminado { get; set; }

    public virtual ICollection<apartado> apartado { get; set; } = new List<apartado>();

    public virtual ICollection<detalleUsuario> detalleUsuarioidAspiranteNavigation { get; set; } = new List<detalleUsuario>();

    public virtual ICollection<detalleUsuario> detalleUsuarioidDirectorNavigation { get; set; } = new List<detalleUsuario>();

    public virtual ICollection<grupoDirector> grupoDirector { get; set; } = new List<grupoDirector>();


    [DisplayName("Grupo artistico:")]
    public virtual grupo? idGrupoNavigation { get; set; }

    [DisplayName("Tipo de usuario:")]
    public virtual tipoUsuario? idTipoNavigation { get; set; }
}
