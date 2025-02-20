namespace ApiUsuarios.Models
{
    public class LoginModel
    {
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; } // Se asume que viene encriptada
    }
}
