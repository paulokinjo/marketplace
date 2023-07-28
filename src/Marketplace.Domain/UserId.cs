using Marketplace.Framework;

namespace Marketplace.Domain
{
    public sealed class UserId : Value<UserId>
    {
        public Guid Value { get; protected set; }

        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value),
                    "User id cannot be null");
            }

            this.Value = value;
        }

        public static implicit operator Guid(UserId self) => self.Value;

        public UserId()
        {

        }
    }
}
