using Marketplace.Framework;

namespace Marketplace.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }

    public class InMemoryCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<Currency> currencies =
           new[]
           {
                new Currency
                {
                    CurrencyCode = "EUR",
                    DecimalPlaces = 2,
                    InUse = true
                },
                new Currency
                {
                    CurrencyCode = "USD",
                    DecimalPlaces = 2,
                    InUse = true
                },
                new Currency
                {
                    CurrencyCode = "JPY",
                    DecimalPlaces = 0,
                    InUse = true
                },
                new Currency
                {
                    CurrencyCode = "DEM",
                    DecimalPlaces = 2,
                    InUse = false
                }
           };
        public Currency FindCurrency(string currencyCode)
        {
            var currency = currencies.FirstOrDefault(c =>
                c.CurrencyCode == currencyCode);

            return currency ?? Currency.None;
        }
    }
}
