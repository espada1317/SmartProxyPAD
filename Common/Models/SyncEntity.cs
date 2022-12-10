using System;

namespace Common.Models
{
    public class SyncEntity
     {
          public Guid ID { get; set; }
          public DateTime LastChange { get; set; }
          public string JsonData { get; set; }
          public string SyncType { get; set; }
          public string ObjectType { get; set; }
          public string Origin { get; set; }
     }
}
