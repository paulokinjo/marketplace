namespace Marketplace.Api
{
    public interface IHandlerCommand<in T>
    {
        Task Handle(T command);
    }
}
