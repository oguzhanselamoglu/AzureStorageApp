using System;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace StorageService.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(string queueName)
        {
            _queueClient = new QueueClient(ConnectionStrings.AzureStorageConnectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessage(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }
        public async Task<QueueMessage?> RetrieveMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                var messages = await _queueClient.ReceiveMessageAsync();
                if (messages.Value != null)
                {
                    return messages.Value;
                }

            }
            return null;
        }
    }
}

