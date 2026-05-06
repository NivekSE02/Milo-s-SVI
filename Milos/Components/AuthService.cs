using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext db, ILogger<AuthService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Usuario?> ValidateUserAsync(string correo, string password)
    {
        var user = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo && u.Estado == 1);

        if (user == null)
            return null;

        bool ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        return ok ? user : null;
    }

    public async Task<Usuario?> Login(string correo, string password)
    {
        var user = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo);

        if (user == null)
            return null;

        // ⚠️ USA ESTO SOLO SI NO HASHEAS (temporal)
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}