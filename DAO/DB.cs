using MongoDB.Driver;
using System.Configuration;

namespace SynWord_Server_CSharp.DAO {
    public class DB {
        public static IMongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
    }
}
