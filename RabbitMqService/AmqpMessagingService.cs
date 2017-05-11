using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqService
{
    public class AmqpMessagingService
    {
        private string _hostName = "localhost";
        private string _userName = "guest";
        private string _password = "guest";
        private string _exchangeName = "";
        private string _oneWayMessageQueueName = "OneWayMessageQueueName";
        private bool _durable = true;


        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = _hostName;
            connectionFactory.UserName = _userName;
            connectionFactory.Password = _password;

            return connectionFactory.CreateConnection();
        }

        public void SetUpQueueForOneMessageDemo(IModel model)
        {
            model.QueueDeclare(_oneWayMessageQueueName, _durable, false, false, null);
        }

        //metodo de envio
        public void SendOneWayMessage(string message, IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.SetPersistent(_durable);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_exchangeName, _oneWayMessageQueueName, basicProperties, messageBytes);
        }


        //metodo de recebimento
        public void ReceiveOneWayMessages(IModel model)
        {
            //BasicQos - qualidade do serviço - Esses parametros podem ser configurados para receber mensagens em lote
            model.BasicQos(0, 1, false);

            // QueueingBasicConsumer é construido em rabbitMq
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            //BasicConsume consome fila especificada
            model.BasicConsume(_oneWayMessageQueueName, false, consumer);
            while (true)
            {
                BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                Console.WriteLine("Message received: {0}", message);
                model.BasicAck(deliveryArguments.DeliveryTag, false);
            }

            
        }


    }
}
