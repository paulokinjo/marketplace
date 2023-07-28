using Marketplace.Domain;
using Marketplace.Mongo;

namespace Marketplace
{
    public class ClassifiedAdRepository
        : IClassifiedAdRepository, IDisposable
    {
        private readonly IMongoRepository<ClassifiedAd> mongoClassifiedAd;

        public ClassifiedAdRepository(IMongoRepository<ClassifiedAd> mongoClassifiedAd)
        {
            this.mongoClassifiedAd = mongoClassifiedAd;
        }

        public async Task<bool> Exists(ClassifiedAdId id)
            => await mongoClassifiedAd.FindOneAsync(c => c.AggregateId == id) != null;

        public async Task<ClassifiedAd> Load(ClassifiedAdId id)
            => await mongoClassifiedAd.FindOneAsync(c => c.AggregateId == id);

        public async Task Add(ClassifiedAd entity) =>
            await mongoClassifiedAd.InsertOneAsync(entity);

        public void Dispose() 
        {
        }

        private static string EntityId(ClassifiedAdId id)
            => $"ClassifiedAd/{id}";

        public async Task Update(ClassifiedAd classifiedAd) => await mongoClassifiedAd.ReplaceOneAsync(classifiedAd);
    }
}
