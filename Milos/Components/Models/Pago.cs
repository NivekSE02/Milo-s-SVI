using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Pago")]
public class Pago
{
    [Key] public int IdPago { get; set; }
    public string Metodo { get; set; } = "";
    public decimal Monto { get; set; }
    public DateTime Fecha_Pago { get; set; } = DateTime.Now;
}