﻿using System;
using System.Collections.Generic;

namespace Armoniza.Domain.Entities;

public partial class apartado
{
    public int id { get; set; }

    /// <summary>
    /// Fecha en la que se entrego el instrumento
    /// </summary>
    public DateTime fechadado { get; set; }

    /// <summary>
    /// fecha estipulada para devolver el instrumento, máximo un semestre
    /// </summary>
    public DateTime fecharegreso { get; set; }

    /// <summary>
    /// Si el apartado esta activo (Aun no devuelto) o ya se libero
    /// </summary>
    public bool activo { get; set; }

    /// <summary>
    /// Id del usuario que pidió el apartado
    /// </summary>
    public int idusuario { get; set; }

    public DateTime? fecharetornado { get; set; }

    public virtual ICollection<detalleApartado> detalleApartado { get; set; } = new List<detalleApartado>();

    public virtual usuario? idUsuarioNavigation { get; set; }
}
