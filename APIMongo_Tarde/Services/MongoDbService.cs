using MongoDB.Driver;

namespace APIMongo_Tarde.Services
{
    public class MongoDbService
    {
        // Armazenar a Configuracao da Aplicacao
        private readonly IConfiguration _configuration;

        
        // Armazena a Referencia ao MongoDb
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Criar um Construtor para Conter a Configuracao Necessaria para Acessar o MongoDb
        /// Objeto Contendo toda a Configuracao da Aplicacao
        /// </summary>
        public MongoDbService(IConfiguration configuration)
        {
            // Atribui  a configuracao em _configuration
            _configuration = configuration;

            // Acessa a string de conexao
            var connectionString = _configuration.GetConnectionString("DbConnection");

            // Transforma a string obtida em MongoUrl
            var mongoUrl = MongoUrl.Create(connectionString);

            // Cria um Client 
            var mongoClient = new MongoClient(mongoUrl);

            // Obtem a referencia com ao MongoDb
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Propriedade para acessar o bd => retorna os dados em _database
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
