using MongoDB.Driver;

namespace Rest.DBContext
{
    public class DBContext
    {
        private static IMongoDatabase _db = null;
        private static readonly object objLock = new object();

        public static IMongoDatabase GetInstance()
        {
            if (_db == null)
            {
                lock (objLock)
                {
                    if (_db == null)
                    {
                        _db = CreateDB();
                    }
                }

            }
            return _db;
        }
        private static IMongoDatabase CreateDB()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017/");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Solar-Power-Generation");
            return database;

        }
    }
}
