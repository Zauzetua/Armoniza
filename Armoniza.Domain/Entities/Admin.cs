using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public partial class Admin
{
    public int id { get; set; }

    [Required(ErrorMessage = "El campo username es obligatorio.")]
    public string username { get; set; } = null!;
    [Required(ErrorMessage = "El campo contrasena es obligatorio.")]

    public string password { get; set; } = null!;

    public string nombre_completo { get; set; } = null!;

    public string telefono { get; set; } = null!;
}
