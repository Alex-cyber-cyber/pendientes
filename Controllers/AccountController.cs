using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyect.Models;
using Proyect.ViewModels;
using System.Threading.Tasks;

namespace Proyect.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // =====================
        // MÉTODOS DE REGISTRO
        // =====================

        // Muestra el formulario de registro
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Procesa el registro de un nuevo usuario
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Crea una nueva instancia de usuario
                var user = new Users
                {
                    Nombres = model.Nombres,
                    Apellidos = model.Apellidos,
                    UserName = model.Username,
                    Email = model.Email,
                    UserType = model.UserType
                };

                // Crea el usuario en la base de datos con la contraseña proporcionada
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Inicia sesión automáticamente después del registro
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // Redirige a la página principal
                }

                // Muestra los errores en caso de fallo
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model); // Devuelve el formulario de registro con errores
        }

        // ==========================
        // MÉTODOS DE INICIO DE SESIÓN (LOGIN)
        // ==========================

        // Muestra el formulario de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa el inicio de sesión del usuario
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Intenta iniciar sesión con el nombre de usuario y contraseña proporcionados
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Redirige a la página principal tras iniciar sesión correctamente
                }

                // Muestra mensaje en caso de cuenta bloqueada
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "La cuenta está bloqueada.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
                }
            }

            return View(model); // Devuelve el formulario de inicio de sesión con mensajes de error
        }

        // =========================
        // MÉTODO DE CIERRE DE SESIÓN (LOGOUT)
        // =========================

        // Procesa el cierre de sesión del usuario
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Cierra la sesión actual
            return RedirectToAction("Login", "Account"); // Redirige a la página de inicio de sesión
        }
    }
}
