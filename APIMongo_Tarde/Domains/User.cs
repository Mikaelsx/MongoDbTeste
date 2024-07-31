using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIMongo_Tarde.Domains
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public int Password { get; set; }

        // Para Adicionar mais Campos essa configuracao e necessaria
        public Dictionary<string, string> AddtionalAttributes { get; set; }

        public User()
        {
            AddtionalAttributes = new Dictionary<string, string>();
        }
    }
}
