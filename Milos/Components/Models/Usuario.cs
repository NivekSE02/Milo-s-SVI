using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Usuario")]
public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string PasswordHash { get; set; }
    public int Estado { get; set; }
    public int IdRol { get; set; }
}