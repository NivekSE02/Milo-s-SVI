namespace Milos.Components;

public class AuthState
{
    public bool IsAuthenticated { get; set; }
    public Usuario? CurrentUser { get; set; }
}
