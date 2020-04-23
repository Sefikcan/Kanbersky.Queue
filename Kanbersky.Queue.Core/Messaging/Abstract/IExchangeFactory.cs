using RabbitMQ.Client;

namespace Kanbersky.Queue.Core.Messaging.Abstract
{
    public interface IExchangeFactory<TModel> where TModel : class
    {
        void CreateExchangeAndSend(TModel model);
    }
}
