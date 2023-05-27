using Marketplace.Framework;

namespace Marketplace.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}
