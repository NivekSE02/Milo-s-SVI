public class AuthState
{
    private bool _isAuthenticated = false;

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        private set
        {
            _isAuthenticated = value;
            OnChange?.Invoke();
        }
    }

    public Usuario? CurrentUser { get; private set; }
    public bool EsAdmin => CurrentUser?.IdRol == 1; // Rol 1 = Admin

    public event Action? OnChange;

    public void Login(Usuario user)
    {
        CurrentUser = user;
        IsAuthenticated = true;
    }

    public void Logout()
    {
        CurrentUser = null;
        IsAuthenticated = false;
    }
}