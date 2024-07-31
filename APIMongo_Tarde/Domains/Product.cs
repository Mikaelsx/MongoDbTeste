using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIMongo_Tarde.Domains
{
    public class Product
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get;set;}

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        // Para Adicionar mais Campos essa configuracao e necessaria
        public Dictionary<string, string> AddtionalAttributes { get; set; }

        public Product() 
        { 
            AddtionalAttributes = new Dictionary<string, string>();
        }
    }
}
