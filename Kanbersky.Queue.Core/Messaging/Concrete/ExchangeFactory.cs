using Kanbersky.Queue.Core.Constants;
using Kanbersky.Queue.Core.Helpers.Messaging;
using Kanbersky.Queue.Core.Messaging.Abstract;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Kanbersky.Queue.Core.Messaging.Concrete
{
    public class ExchangeFactory<TModel> : IExchangeFactory<TModel> where TModel: class
    {
        public void CreateExchangeAndSend(TModel model)
        {
            var connection = RabbitHelper.GetConnection;
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: $"{typeof(TModel).Name}{RabbitMQConstants.BaseExchangeName}",
                type: ExchangeType.Direct,
                durable: true, // veri kaybı olmaması için queueu'daki bilgi diske yazılması için true set ettik
                autoDelete: false, // silme işlemini biz handle edip rabbitmq'ya bildireceğiz.
                arguments: null
                );

            channel.QueueDeclare(
                queue: $"{typeof(TModel).Name}{RabbitMQConstants.BaseQueueName}",
                durable: true, // veri kaybı olmaması için queueu'daki bilgi diske yazılması için true set ettik
                exclusive: false, // İlgili kuyruğa birden fazla kanal üzerinden işlem yapılsın mı yapılmasın mı bilgisini buradan yönetiyoruz.
                autoDelete: false, // silme işlemini biz handle edip rabbitmq'ya bildireceğiz.
                arguments: null
                );

            //Exchange tiplere göre buralar değişecek şekilde yapı kurulabilir !

            channel.QueueBind(
                queue: $"{typeof(TModel).Name}{RabbitMQConstants.BaseQueueName}",
                exchange: $"{typeof(TModel).Name}{RabbitMQConstants.BaseExchangeName}",
                routingKey: $"{typeof(TModel).Name}{RabbitMQConstants.BaseRoutingKeyName}",
                arguments: null
                );

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; //mesajın silinmesin kontrolünü veriyoruz.Biz bildireceğiz bunu rabbitmq'ya

            string serializedMessage = JsonConvert.SerializeObject(model);
            byte[] byteMessage = Encoding.UTF8.GetBytes(serializedMessage);

            channel.BasicPublish(
                exchange: $"{typeof(TModel).Name}{RabbitMQConstants.BaseExchangeName}",
                routingKey: $"{typeof(TModel).Name}{RabbitMQConstants.BaseRoutingKeyName}",
                mandatory: true,
                basicProperties: properties,
                body: byteMessage
                );
        }
    }
}
