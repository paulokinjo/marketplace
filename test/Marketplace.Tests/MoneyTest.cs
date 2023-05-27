using Marketplace.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Marketplace.Domain.Money;

namespace Marketplace.Tests
{
    [TestClass]
    public class MoneyTest
    {
        private static readonly ICurrencyLookup currencyLookup =
            new FakeCurrencyLookup();

        [TestMethod]
        public void TowOfSameAmountShouldBeEqual()
        {
            var firstAmount = FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = FromDecimal(5, "EUR", currencyLookup);
            Assert.AreEqual(firstAmount, secondAmount);
        }

        [TestMethod]
        public void TwoOfSameAmountButDifferentCurrenciesShouldNotBeEqual()
        {
            var firstAmount = FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = FromDecimal(5, "USD", currencyLookup);


            Assert.AreNotEqual(firstAmount, secondAmount);
        }

        [TestMethod]
        public void FromStringAndFromDecimalShouldBeEqual()
        {
            var firstAmount = FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = FromString("5.00", "EUR", currencyLookup);
        
            Assert.AreEqual(firstAmount, secondAmount);
        }

        [TestMethod]
        public void SumOfMoneyGivesFullAmount()
        {
            var coin1 = FromDecimal(1, "EUR", currencyLookup);
            var coin2 = FromDecimal(2, "EUR", currencyLookup);
            var coin3 = FromDecimal(2, "EUR", currencyLookup);

            var bankNote = FromDecimal(5, "EUR", currencyLookup);

            Assert.AreEqual(coin1 + coin2 + coin3, bankNote);
        }

        [TestMethod]
        public void UnusedCurrencyShouldNotBeAllowed()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                FromDecimal(100, "DEM", currencyLookup));
        }

        [TestMethod]
        public void UnknownCurrencyShouldNotBeAllowed()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                FromDecimal(100, "WAHT?", currencyLookup));
        }

        [TestMethod]
        public void ThrowWhenTooManyDecimalPlaces()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                FromDecimal(100.123m, "EUR", currencyLookup));
        }

        [TestMethod]
        public void ThrowsOnAddingDifferentCurrencies()
        {
            var firstAmount = FromDecimal(5, "USD", currencyLookup);
            var secondAmount = FromDecimal(5, "EUR", currencyLookup);

            Assert.ThrowsException<CurrencyMismatchException>(() =>
                firstAmount + secondAmount);
        }

        [TestMethod]
        public void ThrowsOnSubtractingDifferentCurrencies()
        {
            var firstAmount = FromDecimal(5, "USD", currencyLookup);
            var secondAmount = FromDecimal(5, "EUR", currencyLookup);

            Assert.ThrowsException<CurrencyMismatchException>(() =>
                firstAmount - secondAmount);
        }
    }
}
