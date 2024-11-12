using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyect.Data;
using Proyect.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
// Agregar DbContext con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Configuración de Identity
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuración de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Sirve archivos estáticos como CSS, JS, imágenes, etc.
app.UseStaticFiles();

app.UseRouting();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configuración de rutas
app.MapControllerRoute(
    name: "default",
  pattern: "{controller=Account}/{action=Register}/{id?}");

// Iniciar la aplicación
app.Run();
