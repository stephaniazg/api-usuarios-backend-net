using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiUsuarios.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }

        public DateTime? FechaUltimoAcceso { get; set; }
        public int? Puntaje { get; set; }
        public string? Clasificacion { get; set; }


    }
}
