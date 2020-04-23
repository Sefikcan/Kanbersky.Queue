using Kanbersky.Queue.Core.Constants;
using Kanbersky.Queue.Core.Helpers.Messaging;
using Kanbersky.Queue.Service.DTO.Response;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Kanbersky.Queue.Consumer
{
    class Program
    {
        public static bool EmailSend(string email)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();

                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = new MailAddress("dasd@dsada.com");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Konu Tarafı";
                mailMessage.Body = "Body Kısmı";
                mailMessage.IsBodyHtml = true;

                smtpClient.Host = "mail.blabla@dsaa.com";
                smtpClient.Port = 587;

                smtpClient.Credentials = new NetworkCredential("dasda@dasda.com", "2134");
                smtpClient.Send(mailMessage);

                Console.WriteLine($"E-mail gönderilmiştir.{email} adresine gönderilmiştir");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mail gönderiminde hata oluştu.Hata sebebi:{ex.InnerException}");
                return false;
            }
        }

        static void Main(string[] args)
        {
            var result = false;
            var connection = RabbitHelper.GetConnection;
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: $"{typeof(CreateCustomerResponse).Name}{RabbitMQConstants.BaseExchangeName}",
                type: ExchangeType.Direct,
                durable: true, 
                autoDelete: false, 
                arguments: null
                );

            channel.QueueBind(
                queue: $"{typeof(CreateCustomerResponse).Name}{RabbitMQConstants.BaseQueueName}",
                exchange: $"{typeof(CreateCustomerResponse).Name}{RabbitMQConstants.BaseExchangeName}", 
                routingKey: $"{typeof(CreateCustomerResponse).Name}{RabbitMQConstants.BaseRoutingKeyName}", 
                arguments: null
                );
            
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(
                $"{typeof(CreateCustomerResponse).Name}{RabbitMQConstants.BaseQueueName}", 
                false, 
                consumer);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    Console.WriteLine("Kuyruktan mesaj alındı.İşleniyor..");

                    string deserializedString = Encoding.UTF8.GetString(ea.Body);
                    var deserializedModel = JsonConvert.DeserializeObject<CreateCustomerResponse>(deserializedString);

                    result = EmailSend(deserializedModel.Email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata Meydana Geldi:" + ex.Message);
                }

                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            Console.ReadKey();
        }
    }
}
