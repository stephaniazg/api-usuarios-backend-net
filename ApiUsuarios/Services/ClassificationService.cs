using ApiUsuarios.Interfaz;
using System;

namespace ApiUsuarios.Services
{
    public class ClassificationService : IClassificationService
    {
        public string GetClassification(DateTime? ultimoAcceso)
        {
            if (!ultimoAcceso.HasValue)
                return "";

            var diff = DateTime.UtcNow - ultimoAcceso.Value;

            if (diff.TotalHours <= 12)
                return "Hechicero";
            else if (diff.TotalHours <= 48)
                return "Luchador";
            else if (diff.TotalDays <= 7)
                return "Explorador";
            else
                return "Olvidado";
        }
    }
}
