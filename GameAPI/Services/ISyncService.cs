using Common.Models;
using System.Net.Http;

namespace GameAPI.Services
{
    public interface ISyncService<T> where T: MongoDoc
     {
          HttpResponseMessage Upsert(T game);
          HttpResponseMessage Delete(T game);
     }
}
