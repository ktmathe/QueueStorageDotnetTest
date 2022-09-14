using System.Configuration; // Namespace for ConfigurationManager
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage

/// <summary>
/// https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet#use-additional-options-for-dequeuing-messages
/// </summary>
class Program 
{
    static public void Main()
    {
        var queueName = "testqueue";

        var client = CreateQueueClient(queueName);

        // Send a message to the queue
        var message = "such message";
        client.SendMessage(message);
        Console.WriteLine($"Inserted: {message}");

        // Peek at the next message
        PeekedMessage[] peekedMessage = client.PeekMessages();
        Console.WriteLine($"Peeked message: '{peekedMessage[0].Body}'");


        // Get the message from the queue
        QueueMessage[] messages = client.ReceiveMessages();

        // Update the message contents
        client.UpdateMessage(messages[0].MessageId,
                messages[0].PopReceipt,
                "Wow message updated",
                TimeSpan.FromSeconds(60.0)  // Make it invisible for another 60 seconds
                );

        // Get the next message
        QueueMessage[] retrievedMessage = client.ReceiveMessages();

        // Process (i.e. print) the message in less than 30 seconds
        Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

        // Delete the message
        client.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);


        QueueProperties properties = client.GetProperties();

        // Retrieve the cached approximate message count.
        int cachedMessagesCount = properties.ApproximateMessagesCount;

        // Display number of messages.
        Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");


        if (client.Exists())
        {
            // Delete the queue
            client.Delete();
        }

        Console.WriteLine($"Queue deleted: '{client.Name}'");

    }

    //-------------------------------------------------
    // Create the queue service client
    //-------------------------------------------------
    static public QueueClient CreateQueueClient(string queueName)
    {
        // Get the connection string from app settings
        string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

        // Instantiate a QueueClient which will be used to create and manipulate the queue
        var client = new QueueClient(connectionString, queueName);

        client.CreateIfNotExists();

        if (client.Exists())
        {
            Console.WriteLine($"Queue created: '{client.Name}'");
        }

        return client; 
    }
   

}
