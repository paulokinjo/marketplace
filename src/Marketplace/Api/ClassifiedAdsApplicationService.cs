using Marketplace.Domain;
using Marketplace.Framework;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IClassifiedAdRepository repository;
        private ICurrencyLookup currencyLookup;

        public ClassifiedAdsApplicationService(IClassifiedAdRepository repository, ICurrencyLookup currencyLookup)
        {
            this.repository = repository;
            this.currencyLookup = currencyLookup;
        }

        public Task Handle(object command) =>
            command switch
            {
                V1.Create cmd => HandleCreate(cmd),
                V1.SetTitle cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))),
                V1.UpdateText cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.UpdateText(
                            ClassifiedAdText.FromString(cmd.Text))),
                V1.UpdatePrice cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.UpdatePrice(
                            Price.FromDecimal(cmd.Price, cmd.Currency, currencyLookup))),
                V1.RequestToPublish cmd =>
                    HandleUpdate(
                        cmd.Id,
                        c => c.RequestToPublish()),
                _ => Task.CompletedTask
            };
        private async Task HandleCreate(V1.Create cmd)
        {
            var exists = await repository.Exists(new ClassifiedAdId(cmd.Id));
            if (exists)
            {
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
            }

            var classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(cmd.Id),
            new UserId(cmd.OwnerId));

            await repository.Add(classifiedAd);
        }

        private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
        {
            var classifiedAd = await repository.Load(new ClassifiedAdId(classifiedAdId));
            if (classifiedAd == null)
            {
                throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
            }

            operation(classifiedAd);

            await repository.Update(classifiedAd);
        }
    }
}
