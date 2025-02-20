namespace ApiUsuarios.Interfaz
{
    public interface IUserScoreService
    {
        int CalculateScore(string nombre, string apellidos, string correo);
    }
}
