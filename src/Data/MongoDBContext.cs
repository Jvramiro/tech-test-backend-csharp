using MongoDB.Bson;
using MongoDB.Driver;
using TestJrAPI.Models;

namespace TestJrAPI.Data {
    public class MongoDBContext {

        private IMongoDatabase database;

        public MongoDBContext(IConfiguration configuration) {
            try {
                string connectionString = configuration["ConnectionStrings:MongoDB"];
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                var mongoClient = new MongoClient(settings);
                database = mongoClient.GetDatabase(configuration["Database:Name"]);
            }
            catch (Exception ex) {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }

        }

        public IMongoCollection<Produto> Produtos {
            get {
                return database.GetCollection<Produto>("Produtos");
            }
        }

    }
}
