using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Venta")]
public class Venta
{
    [Key] public int IdVenta { get; set; }
    public int IdUsuario { get; set; }
    public int IdPago { get; set; }
    public DateTime FechaPedido { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public string Estado { get; set; } = "";

    [ForeignKey("IdUsuario")] public Usuario? Usuario { get; set; }
    [ForeignKey("IdPago")] public Pago? Pago { get; set; }
    public List<DetalleVenta> Detalles { get; set; } = new();
}