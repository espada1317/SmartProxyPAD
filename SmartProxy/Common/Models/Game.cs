using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
     public class Game: MongoDoc
     {
          public string Name { get; set; }
          public List<string> Developers { get; set; }
          public List<string> Publishers { get; set; }
          public float? Price { get; set; }
     }
}
