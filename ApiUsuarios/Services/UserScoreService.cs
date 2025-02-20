using ApiUsuarios.Interfaz;
using System;

namespace ApiUsuarios.Services
{
    public class UserScoreService : IUserScoreService
    {
        public int CalculateScore(string nombre, string apellidos, string correo)
        {
            /// Calcula el puntaje del usuario según:
            /// - Longitud del nombre completo (Nombre + Apellidos):
            ///   • > 10 caracteres: +20 puntos.
            ///   • Entre 5 y 10: +10 puntos.
            ///   • Menos de 5: +0 puntos.
            /// - Dominio del correo:
            ///   • gmail.com: +40 puntos.
            ///   • hotmail.com: +20 puntos.
            ///   • Otros: +10 puntos.
            
            int puntaje = 0;
            string nombreCompleto = $"{nombre} {apellidos}".Trim();
            int longitud = nombreCompleto.Length;
            if (longitud > 10)
                puntaje += 20;
            else if (longitud >= 5)
                puntaje += 10;

            if (!string.IsNullOrEmpty(correo))
            {
                if (correo.EndsWith("gmail.com", StringComparison.OrdinalIgnoreCase))
                    puntaje += 40;
                else if (correo.EndsWith("hotmail.com", StringComparison.OrdinalIgnoreCase))
                    puntaje += 20;
                else
                    puntaje += 10;
            }

            return puntaje;
        }
    }
}
