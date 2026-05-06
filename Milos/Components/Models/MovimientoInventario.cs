using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("MovimientoInventario")]
public class MovimientoInventario
{
    [Key] public int IdMovimiento { get; set; }
    public string TipoMovimiento { get; set; } = "";
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    [ForeignKey("IdProducto")] public Producto? Producto { get; set; }
}