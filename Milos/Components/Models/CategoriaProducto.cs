using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CategoriaProducto")]
public class CategoriaProducto
{
    [Key] public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = "";
    public string? Descripcion { get; set; }
}