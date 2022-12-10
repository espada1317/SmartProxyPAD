using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
     public abstract class MongoDoc
     {
          [BsonId]
          public Guid ID { get; set; }
          public DateTime LastChange { get; set; }
     }
}
