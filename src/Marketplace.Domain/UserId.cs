using Marketplace.Framework;

namespace Marketplace.Domain
{
    public sealed class UserId : Value<UserId>
    {
        private readonly Guid value;

        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value),
                    "User id cannot be null");
            }

            this.value = value;
        }

        public static implicit operator Guid(UserId self) => self.value;
    }
}
