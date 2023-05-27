using Marketplace.Framework;

namespace Marketplace.Domain
{
    public sealed class ClassifiedAdId : Value<ClassifiedAdId>
    {
        private readonly Guid value;

        public ClassifiedAdId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value),
                    "Classified Ad id cannot be empty");
            }

            this.value = value;
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.value;
        public static implicit operator ClassifiedAdId(Guid self) => self;
    }
}
