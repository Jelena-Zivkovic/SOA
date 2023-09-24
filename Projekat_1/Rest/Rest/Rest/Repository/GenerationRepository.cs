using MongoDB.Bson;
using MongoDB.Driver;
using Rest.Models;

namespace Rest.Repository
{
    public class GenerationRepository : IRepository<Generation>
    {
        private readonly IMongoDatabase _dbContext;
        private readonly IMongoCollection<Generation> coll;
        public GenerationRepository(IMongoDatabase db)
        {
            _dbContext = db;
            coll = db.GetCollection<Generation>("Generation");
        }
        public void Add(Generation gen)
        {
            coll.InsertOne(gen);
        }

        public List<Generation> GetAll()
        {
            return coll.Find(x => true).Limit(100).ToList();
        }

        public Generation GetById(string id)
        {
            return coll.Find(x => x.ID == id).FirstOrDefault(); 
        }

        public void Modify(string id, Generation gen)
        {
            coll.ReplaceOneAsync(x => x.ID == id, gen);
        }

        public void RemoveById(string id)
        {
            coll.DeleteOneAsync(x => x.ID == id);
        }

        public async Task ModifyDailyYieldById(string id, double dailyYield)
        {
            Generation gen = coll.Find(x => x.ID == id).FirstOrDefault();
            gen.Total_yield = gen.Total_yield - gen.Daily_yield + dailyYield;
            gen.Daily_yield = dailyYield;
            await coll.ReplaceOneAsync(x => x.ID == id, gen);
        }
    }
}
