using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DetalleVenta")]
public class DetalleVenta
{
    [Key] public int IdDetalle { get; set; }
    public int IdVenta { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }

    [ForeignKey("IdProducto")] public Producto? Producto { get; set; }
}