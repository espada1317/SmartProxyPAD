using Common.Models;
using Common.Utilities;
using Microsoft.Extensions.Hosting;
using NodeSync.Settings;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NodeSync.Services
{
    public class SyncService: IHostedService
     {
          private readonly ConcurrentDictionary<Guid, SyncEntity> documents = new ConcurrentDictionary<Guid, SyncEntity>();
          private readonly IGameAPISettings _settings; 
          private Timer _timer;

          public SyncService(IGameAPISettings settings)
          {
               _settings = settings;
          }

          public void AddItem(SyncEntity entity)
          {
               SyncEntity document = null;
               bool isPresent = documents.TryGetValue(entity.ID, out document);

               if (!isPresent || (isPresent && entity.LastChange > document.LastChange))
               {
                    documents[entity.ID] = entity;
               }
          }

          public Task StartAsync(CancellationToken cancellationToken)
          {
               _timer = new Timer(SendData, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
               return Task.CompletedTask;
          }

          public Task StopAsync(CancellationToken cancellationToken)
          {
               _timer?.Change(Timeout.Infinite, 0);
               return Task.CompletedTask;
          }

          private void SendData(object state)
          {
               foreach (var doc in documents)
               {
                    SyncEntity entity = null;
                    var isPresent = documents.TryRemove(doc.Key, out entity);
                    if (isPresent)
                    {
                         var receivers = _settings.Hosts.Where(x => !x.Contains(entity.Origin));
                         foreach (var receiver in receivers)
                         {
                              var url = $"{receiver}/{entity.ObjectType}/sync";
                              try
                              {
                                   var result = HttpClientUtility.SendJson(entity.JsonData, url, entity.SyncType);
                              }
                              catch(Exception e)
                              {
                                   Console.WriteLine("Error");
                              }
                         }
                    }
               }
          }
     }
}
