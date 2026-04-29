using Milos.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add application services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthState>();

var app = builder.Build();

// Perform a startup DB connectivity check and log result
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        logger.LogInformation("Probing database connectivity...");
        var can = await db.Database.CanConnectAsync();
        if (can)
        {
            bool any = false;
            try
            {
                any = await db.Usuarios.AnyAsync();
                logger.LogInformation("Database connected. Usuarios.AnyAsync returned {Any}", any);
            }
            catch (Exception exQuery)
            {
                logger.LogWarning(exQuery, "Database connected but query failed");
            }
        }
        else
        {
            logger.LogWarning("Database.CanConnectAsync returned false");
        }
    }
    catch (Exception ex)
    {
        var logger2 = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger2.LogError(ex, "Database connectivity probe failed at startup");
    }
}

// Add a simple HTTP endpoint to report DB status even if UI isn't interactive
app.MapGet("/dbstatus", async (IServiceProvider sp) =>
{
    using var scope = sp.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var can = await db.Database.CanConnectAsync();
        if (!can)
            return Results.Problem("Database reported it cannot connect (CanConnectAsync==false)");

        try
        {
            var any = await db.Usuarios.AnyAsync();
            return Results.Ok(new { connected = true, usuariosAny = any });
        }
        catch (Exception exQuery)
        {
            logger.LogError(exQuery, "Connected but query failed");
            return Results.Problem("Connected but query failed: " + exQuery.Message);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "DB status endpoint failed");
        return Results.Problem(ex.Message);
    }
});

// Temporary API endpoint to test login via curl/postman
app.MapPost("/apilogin", async (LoginDto login, AuthService auth, ILogger<Program> logger) =>
{
    if (login is null || string.IsNullOrWhiteSpace(login.Correo) || string.IsNullOrWhiteSpace(login.Password))
    {
        return Results.BadRequest(new { success = false, message = "Correo y contraseña son requeridos" });
    }

    logger.LogInformation("API login attempt for {Correo}", login.Correo);
    var user = await auth.ValidateUserAsync(login.Correo, login.Password);
    if (user != null)
    {
        return Results.Ok(new { success = true, message = "Autenticación correcta", userId = user.IdUsuario, nombre = user.Nombre });
    }
    else
    {
        return Results.Unauthorized();
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
