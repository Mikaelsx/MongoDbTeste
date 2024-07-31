using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIMongo_Tarde.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("cpf")]
        public string Cpf { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        // Para Adicionar mais Campos essa configuracao e necessaria
        public Dictionary<string, string> AddtionalAttributes { get; set; }

        public Client()
        {
            AddtionalAttributes = new Dictionary<string, string>();
        }
    }
}
