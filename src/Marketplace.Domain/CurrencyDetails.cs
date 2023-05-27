using Marketplace.Framework;

public class Currency : Value<Currency>
{
    public string CurrencyCode { get; set; } = default!;
    public bool InUse { get; set; }
    public int DecimalPlaces { get; set; }

    public static Currency None = new Currency()
    {
        InUse = false,
    };
}
