using Microsoft.AspNetCore.Identity;


namespace Proyect.Models
{
    public class Users : IdentityUser
    {
        public string Nombres { get; set; } // Nuevo campo
        public string Apellidos { get; set; } // Nuevo campo
        public string UserType { get; set; } // Puede ser "Administrador" o "Usuario"
    }
} 


