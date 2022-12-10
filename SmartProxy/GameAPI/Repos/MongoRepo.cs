using Common.Models;
using GameAPI.Setting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
               game.ID = Guid.NewGuid();
               _collection.InsertOne(game);
               return game;
          }

          public void UpsertGame(T game)
          {
               var result = _collection.Find(doc => doc.ID == game.ID).FirstOrDefault();
               if (result == null)
               {
                    InsertGame(game);
               }
               else
               {
                    _collection.ReplaceOne(doc => doc.ID == game.ID, game);
               }
          }
     }
}
