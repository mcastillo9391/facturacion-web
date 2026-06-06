namespace Facturacion.Domain.Entities;

public class Cliente
{
    public int IdCli { get; set; }

    public string Identificacion { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public bool Activo { get; set; }

    public ICollection<PendienteVenta>
        PendientesVenta
        = new List<PendienteVenta>();
    
    public ICollection<Pago>
    Pagos
    = new List<Pago>();
}