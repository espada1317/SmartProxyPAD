using Common.Models;
using System;
using System.Collections.Generic;

namespace GameAPI.Repos
{
    public interface IMongoRepo<T> where T: MongoDoc
     {
          List<T> GetAllGames();
          T InsertGame(T game);
          T GetGameById(Guid id);
          void UpsertGame(T game);
          void DeleteGame(Guid id);
     }
}