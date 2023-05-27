using Marketplace.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marketplace.Tests
{
    [TestClass]
    public class ClassifiedAdTests
    {
        private readonly ClassifiedAd classifiedAd;

        public ClassifiedAdTests()
        {
            classifiedAd = new(
                 id: new(Guid.NewGuid()),
                 ownerId: new(Guid.NewGuid()));
        }

        [TestMethod]
        public void CanPublishAValidAd()
        {
            classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            classifiedAd.UpdateText(ClassifiedAdText.FromString("Please by my stuff"));
            classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));
            classifiedAd.RequestToPublish();

            Assert.AreEqual(ClassifiedAd.ClassifiedAdState.PendingReview, classifiedAd.State);
        }

        [TestMethod]
        public void CannotPublishWithoutTitle()
        {
            classifiedAd.UpdateText(ClassifiedAdText.FromString("Please by my stuff"));
            classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));
            
            Assert.ThrowsException<InvalidEntityStateException>(classifiedAd.RequestToPublish);
        }

        [TestMethod]
        public void CannotPublishWithoutText()
        {
            classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

            Assert.ThrowsException<InvalidEntityStateException>(classifiedAd.RequestToPublish);
        }

        [TestMethod]
        public void CannotPublishWithoutPrice()
        {
            classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            classifiedAd.UpdateText(ClassifiedAdText.FromString("Please by my stuff"));

            Assert.ThrowsException<InvalidEntityStateException>(classifiedAd.RequestToPublish);
        }

        [TestMethod]
        public void CannotPublishWithZeroPrice()
        {
            classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            classifiedAd.UpdateText(ClassifiedAdText.FromString("Please by my stuff"));
            classifiedAd.UpdatePrice(Price.FromDecimal(0.0m, "EUR", new FakeCurrencyLookup()));

            Assert.ThrowsException<InvalidEntityStateException>(classifiedAd.RequestToPublish);
        }
    }
}
