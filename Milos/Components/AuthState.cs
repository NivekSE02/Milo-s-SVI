namespace Milos.Components;

public class AuthState
{
    private bool _isAuthenticated;

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        private set
        {
            if (_isAuthenticated == value) return;
            _isAuthenticated = value;
            OnChange?.Invoke();
        }
    }

    public Usuario? CurrentUser { get; private set; }

    public event Action? OnChange;

    public void Login(Usuario user)
    {
        CurrentUser = user;
        IsAuthenticated = true; // triggers OnChange
    }

    public void Logout()
    {
        CurrentUser = null;
        IsAuthenticated = false;
    }
}