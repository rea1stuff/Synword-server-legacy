﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SynWord_Server_CSharp.Logging {
    public class UniqueCheckUsageLog {
        readonly private IMongoClient _client;

        public UniqueCheckUsageLog() {
            _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        }

        public void CheckIpExistsIfNotThenCreate(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingUniquenessChecks");
            BsonDocument filter = new BsonDocument("ip", ip);
            List<BsonDocument> documents = collection.Find(filter).ToList();

            if (documents.Count == 0) {
                UploadIpToDataBase(ip);
            }
        }

        private void UploadIpToDataBase(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingUniquenessChecks");

            BsonDocument document = new BsonDocument{
                { "ip", ip },
                { "usesForAllTime", 0 },
                { "usesForTheLastDay", 0 }
            };

            collection.InsertOne(document);
        }

        public int GetUsesIn24Hours(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingUniquenessChecks");
            BsonDocument filter = new BsonDocument("ip", ip);
            List<BsonDocument> documents = collection.Find(filter).ToList();

            return documents.Count != 0 ? documents[0]["usesForTheLastDay"].ToInt32() : 0;
        }

        public void IncrementNumberOfUsesIn24Hours(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingUniquenessChecks");
            BsonDocument filter = new BsonDocument("ip", ip);
            BsonDocument update = new BsonDocument("$inc", new BsonDocument { { "usesForAllTime", 1 }, { "usesForTheLastDay", 1 } });
            collection.UpdateOne(filter, update);
        }

        public void ResetNumberOfUsesIn24Hours() {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingUniquenessChecks");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> documentList = collection.Find(filter).ToList();

            foreach (BsonDocument document in documentList) {
                filter = new BsonDocument("_id", new ObjectId(Convert.ToString(document["_id"])));
                BsonDocument update = new BsonDocument("$set", new BsonDocument { {"usesForTheLastDay", 0} });
                collection.UpdateOne(filter, update);
            }
        }
    }
}
