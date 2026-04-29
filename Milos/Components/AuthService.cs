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
        try
        {
            _logger.LogDebug("Validating user {Correo}", correo);
            // Find user by correo and active state
            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo && u.Estado == 1);
            if (user == null)
            {
                _logger.LogInformation("No user found for {Correo}", correo);
                return null;
            }

            // Verify BCrypt hash
            bool ok = false;
            try
            {
                ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error verifying password for {Correo}", correo);
                ok = false;
            }

            if (ok)
                _logger.LogInformation("User {Correo} authenticated", correo);
            else
                _logger.LogInformation("Invalid password for {Correo}", correo);

            return ok ? user : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during ValidateUserAsync");
            throw;
        }
    }

    public async Task<(bool Connected, string? ErrorMessage)> TestConnectionAsync()
    {
        try
        {
            _logger.LogInformation("Testing database connectivity...");
            bool can = await _db.Database.CanConnectAsync();
            if (!can)
            {
                _logger.LogWarning("Database.CanConnectAsync returned false");
                return (false, "Database reported it cannot connect (CanConnectAsync==false)");
            }

            // Try a simple query to ensure EF can execute SQL and map results
            try
            {
                var any = await _db.Usuarios.AnyAsync();
                _logger.LogInformation("Database reachable, Usuarios.AnyAsync returned {Any}", any);
                return (true, $"Conectado a la base de datos. Usuarios.AnyAsync returned {any}");
            }
            catch (Exception exQuery)
            {
                _logger.LogError(exQuery, "Connected but query failed");
                return (false, "Conectar OK pero la consulta falló: " + exQuery.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connectivity test failed");
            return (false, ex.ToString());
        }
    }

    // Simple wrapper to match existing working example
    public Task<Usuario?> Login(string correo, string password)
    {
        return ValidateUserAsync(correo, password);
    }
}
