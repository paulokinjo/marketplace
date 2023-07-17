using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Marketplace.Framework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName) => CollectionName = collectionName;
    }
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }
    }

    public abstract class Entity : IDocument
    {
        private readonly List<object> events;

        private ObjectId _Id;
        public ObjectId Id
        {
            get => _Id;
            set
            {
                _Id = value;
                CreatedAt = Id.CreationTime;
            }
        }

        [BsonExtraElements]
        public Dictionary<string, object> ExtraElements { get; set; }

        public DateTime CreatedAt { get; set; }

        protected Entity() => events = new List<object>();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            events.Add(@event);
        }

        protected abstract void When(object @event);
        protected abstract void EnsureValidState();

        public IEnumerable<object> GetChanges() => events.AsEnumerable();

        public void ClearChanges() => events.Clear();
    }
}
