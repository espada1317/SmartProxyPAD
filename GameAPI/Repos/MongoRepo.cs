using Common.Models;
using GameAPI.Setting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameAPI.Repos
{
    public class MongoRepo<T> : IMongoRepo<T> where T : MongoDoc
     {
          public readonly IMongoDatabase _db;
          public readonly IMongoCollection<T> _collection;
          public MongoRepo(IMongoDbSettings dbSettings)
          {
               _db = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);
               string tableName = typeof(T).Name.ToLower();
               _collection = _db.GetCollection<T>(tableName);
          }

          public void DeleteGame(Guid id)
          {
               _collection.DeleteOne(doc => doc.ID == id);
          }

          public List<T> GetAllGames()
          {
               var games = _collection.Find(new BsonDocument()).ToList();
               return games;
          }

          public T GetGameById(Guid id)
          {
               var result = _collection.Find(doc => doc.ID == id).FirstOrDefault();
               return result;
          }

          public T InsertGame(T game)
          {
               _collection.InsertOne(game);
               return game;
          }

          public void UpsertGame(T game)
          {
               _collection.ReplaceOne(doc => doc.ID == game.ID, game, new ReplaceOptions() { IsUpsert = true });
          }
     }
}
