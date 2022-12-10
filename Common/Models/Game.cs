namespace Common.Models
{
    public class Game: MongoDoc
     {
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public float? Price { get; set; }
        public float? Rating { get; set; }
     }
}
