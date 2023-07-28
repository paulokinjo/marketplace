using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Marketplace.Framework
{
    public abstract class Document : IDocument
    {
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

        public override int GetHashCode() =>
            (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public static bool operator ==(Document a, Document b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Document a, Document b) => !(a == b);

        public override bool Equals(object? obj)
        {
            var comparareTo = obj as Document;

            if (ReferenceEquals(this, comparareTo))
            {
                return true;
            }

            if (comparareTo is null)
            {
                return false;
            }

            return Id.Equals(comparareTo.Id);
        }

        public override string ToString() => $"{GetType().Name} [Id={Id}]";
    }
}
