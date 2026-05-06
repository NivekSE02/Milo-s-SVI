using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Producto")]
public class Producto
{
    [Key] public int IdProducto { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal? PrecioMayoreo { get; set; }
    public string? Talla { get; set; }
    public int Stock { get; set; }
    public string? ImagenURL { get; set; }
    public int IdCategoria { get; set; }

    [ForeignKey("IdCategoria")]
    public CategoriaProducto? Categoria { get; set; }
}