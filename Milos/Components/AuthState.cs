public class AuthState
{
    public bool IsAuthenticated { get; private set; } = false;
    public int IdUsuario { get; private set; }
    public string Nombre { get; private set; } = "";
    public int IdRol { get; private set; }
    public bool EsAdmin => IdRol == 1;

    public event Action? OnChange;

    public void Login(Usuario user)
    {
        IdUsuario = user.IdUsuario;
        Nombre = user.Nombre;
        IdRol = user.IdRol;
        IsAuthenticated = true;
        OnChange?.Invoke();
    }

    public void Logout()
    {
        IdUsuario = 0;
        Nombre = "";
        IdRol = 0;
        IsAuthenticated = false;
        OnChange?.Invoke();
    }
}