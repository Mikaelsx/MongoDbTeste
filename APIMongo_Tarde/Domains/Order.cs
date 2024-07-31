using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIMongo_Tarde.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [BsonElement("clientId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }

        // Adicionando propriedades para armazenar os produtos e clientes associados
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Client> Clients { get; set; } = new List<Client>();
    }
}
