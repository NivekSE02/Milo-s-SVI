using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Exportacion_Factura")]
public class ExportacionFactura
{
    [Key] public int IdExportacion { get; set; }
    public int Estado { get; set; } = 1;
    public DateTime? Fecha { get; set; } = DateTime.Now;
    public int IdVenta { get; set; }
    public string? NumeroFactura { get; set; }

    [ForeignKey("IdVenta")] public Venta? Venta { get; set; }
}