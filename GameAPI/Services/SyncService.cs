using Common.Models;
using Common.Utilities;
using GameAPI.Setting;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;

namespace GameAPI.Services
{
    public class SyncService<T> : ISyncService<T> where T : MongoDoc
     {
          private readonly ISyncServiceSettings _syncSettings;
          private readonly IHttpContextAccessor _httpContext;
          public SyncService(ISyncServiceSettings settings, IHttpContextAccessor context)
          {
               _syncSettings = settings;
               _httpContext = context;
          }

          public HttpResponseMessage Delete(T game)
          {
               var syncType = _syncSettings.DeleteHttpMethod;
               var json = ToSyncEntityJson(game, syncType);

               var response = HttpClientUtility.SendJson(json, _syncSettings.Host, "POST");
               return response;
          }

          public HttpResponseMessage Upsert(T game)
          {
               var syncType = _syncSettings.UpsertHttpMethod;
               var json = ToSyncEntityJson(game, syncType);

               var response = HttpClientUtility.SendJson(json, _syncSettings.Host, "POST");
               return response;
          }

          private string ToSyncEntityJson(T game, string syncType)
          {
               var objectType = typeof(T);
               var syncEntity = new SyncEntity
               {
                    JsonData = JsonSerializer.Serialize(game),
                    SyncType = syncType,
                    ObjectType = objectType.Name,
                    ID = game.ID,
                    LastChange = game.LastChange,
                    Origin = _httpContext.HttpContext.Request.Host.ToString()
               };
               var json = JsonSerializer.Serialize(syncEntity);
               return json;
          }
     }
}
