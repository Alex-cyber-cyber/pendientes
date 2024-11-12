using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Proyect.Data;
using Proyect.Models;
using System;

namespace Proyect
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método se llama en tiempo de ejecución para configurar los servicios.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configura el DbContext con Entity Framework y la cadena de conexión
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Configura ASP.NET Identity para usar el modelo de usuario personalizado y Entity Framework
            services.AddIdentity<Users, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configuración de cookies de autenticación
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Ruta a la página de inicio de sesión
                options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta a la página de acceso denegado
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Duración de la cookie
                options.SlidingExpiration = true; // La cookie se renueva si el usuario sigue activo
            });

            // Agrega controladores con vistas (MVC)
            services.AddControllersWithViews();

            // Agrega soporte para sesiones
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // Este método se llama en tiempo de ejecución para configurar el middleware.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Agrega autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();

            // Agrega soporte de sesión
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                // Configura la ruta predeterminada para las páginas
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
