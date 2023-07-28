using Marketplace.Framework;

namespace Marketplace.Domain
{
    public sealed class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public Guid Value { get; protected set; }


        public ClassifiedAdId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value),
                    "Classified Ad id cannot be empty");
            }

            this.Value = value;
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
        public static implicit operator ClassifiedAdId(Guid self) => self;

        public ClassifiedAdId()
        {

        }
    }
}
