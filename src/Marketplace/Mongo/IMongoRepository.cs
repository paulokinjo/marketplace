using Marketplace.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Marketplace.Mongo
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> collection;

        public MongoRepository(AppSettings settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.Mongo.DatabaseName);

            // Set up MongoDB conventions
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
        };

            collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected static string GetCollectionName(Type documentType) =>
           ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                   typeof(BsonCollectionAttribute),
                   true)
               .FirstOrDefault())?.CollectionName;

        public virtual IQueryable<TDocument> AsQueryable() => collection.AsQueryable();

        public virtual async Task<IEnumerable<TDocument>> FindAsync(FilterDefinition<TDocument> filter) => await collection.Find(filter).ToListAsync();
        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression) => collection.Find(filterExpression).ToEnumerable();

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression) => collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();

        public virtual Task<List<TDocument>> FilterByAsync(
         Expression<Func<TDocument, bool>> filterExpression) => collection.Find(filterExpression).ToListAsync();

        public virtual Task<List<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression) => collection.Find(filterExpression).Project(projectionExpression).ToListAsync();

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression) => collection.Find(filterExpression).FirstOrDefault();

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression) => collection.Find(filterExpression).FirstOrDefaultAsync();

        public virtual TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                Expression<Func<TDocument, bool>> expression = g => g.Id == objectId;
                return collection.Find(expression).SingleOrDefaultAsync();
            }

            throw new ArgumentException($"{nameof(id)} cannot be parsed to MongoDB.Bson ObjectId");
        }

        public virtual Task<TDocument> FindByIdAsync(ObjectId objectId)
        {
            Expression<Func<TDocument, bool>> expression = g => g.Id == objectId;

            return collection.Find(expression).SingleOrDefaultAsync();
        }


        public virtual void InsertOne(TDocument document) => collection.InsertOne(document);

        public virtual Task InsertOneAsync(TDocument document) => collection.InsertOneAsync(document);

        public void InsertMany(ICollection<TDocument> documents) => collection.InsertMany(documents);

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents) => await collection.InsertManyAsync(documents);

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression) => collection.FindOneAndDelete(filterExpression);

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression) => collection.FindOneAndDeleteAsync(filterExpression);

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return collection.FindOneAndDeleteAsync(filter);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression) => collection.DeleteMany(filterExpression);

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression) => collection.DeleteManyAsync(filterExpression);
        public Task<IClientSession> StartSessionAsync() => throw new NotImplementedException();
    }

    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();
        Task<IEnumerable<TDocument>> FindAsync(FilterDefinition<TDocument> filter);
        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        Task<List<TDocument>> FilterByAsync(
         Expression<Func<TDocument, bool>> filterExpression);

        Task<List<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(string id);

        Task<TDocument> FindByIdAsync(ObjectId id);

        void InsertOne(TDocument document);

        Task InsertOneAsync(TDocument document);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(ICollection<TDocument> documents);

        void ReplaceOne(TDocument document);

        Task ReplaceOneAsync(TDocument document);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<IClientSession> StartSessionAsync();
    }

}
