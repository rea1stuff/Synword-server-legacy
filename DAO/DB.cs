using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.DAO {
    public class DB {
        public static IMongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
    }
}