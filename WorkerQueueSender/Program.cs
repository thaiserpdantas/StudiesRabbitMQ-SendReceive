using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMqService;

namespace WorkerQueueSender
{
    class Program
    {
        static void Main(string[] args)
        {
            AmqpMessagingService messagingService = new AmqpMessagingService();
            IConnection connection = messagingService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();
            messagingService.SetUpQueueForWorkerQueueDemo(model);

            RunWorkerQueueMessageDemo(model, messagingService);
            Console.ReadKey();

        }

        private static void RunWorkerQueueMessageDemo(IModel model, AmqpMessagingService messagingService)
        {
            Console.WriteLine("Enter your message and press enter. Quit with 'q'.");
            while (true)
            {
                string message = Console.ReadLine();
                if (message.ToLower() == "q") break;
                messagingService.SendMessageToWorkerQueue(message, model);
            }
        }


    }
}
