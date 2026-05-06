using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Rol")]
public class Rol
{
    [Key] public int IdRol { get; set; }
    public string Tipo { get; set; } = "";
    public int Permisos { get; set; }
}