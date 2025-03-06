using System;
using System.Collections.Generic;

namespace Armoniza.Infrastructure.Infrastructure.Data;

public partial class detalleApartado
{
    public int id { get; set; }

    public int idInstrumento { get; set; }

    public int idApartado { get; set; }

    public virtual apartado idApartadoNavigation { get; set; } = null!;

    public virtual instrumento idInstrumentoNavigation { get; set; } = null!;
}
