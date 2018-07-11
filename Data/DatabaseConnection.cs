using MongoDB.Driver;
using Web.Model;

namespace Web.Data
{
    public class DatabaseConnection
    {
        private readonly IMongoDatabase _database = null;

        public DatabaseConnection() //IOptions<Settings> settings)
        {
            var client = new MongoClient("mongodb://localhost"); //settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase("todoDotNet"); //settings.Value.Database);
        }

        public IMongoCollection<ToDoList> Lists => _database.GetCollection<ToDoList>("todolists");

        //public IMongoCollection<ToDoItem> Items => _database.GetCollection<ToDoItem>("items");
    }
}