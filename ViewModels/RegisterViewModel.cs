namespace Proyect.ViewModels
{
    public class RegisterViewModel
    {
        public string Nombres { get; set; } // Nuevo campo
        public string Apellidos { get; set; } // Nuevo campo
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } // Puede ser "Administrador" o "Usuario"
    }
}
