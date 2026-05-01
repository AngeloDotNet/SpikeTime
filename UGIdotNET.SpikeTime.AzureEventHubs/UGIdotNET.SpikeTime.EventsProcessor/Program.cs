using Azure.Data.SchemaRegistry;
using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using com.azure.schemaregistry.samples;
using Microsoft.Azure.Data.SchemaRegistry.ApacheAvro;
using Microsoft.Extensions.Configuration;
using System.Text;

Console.WriteLine("Spike time - events processor");

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var storageAccountConnectionString = configuration.GetConnectionString("AzureStorageAccount");

var eventHubsConnectionString = configuration.GetConnectionString("AzureEventHubs");
const string hubName = "ordershub";

const string schemaRegistryEndpoint = "spiketime-eventhub.servicebus.windows.net";

// name of the consumer group   
const string schemaGroup = "spiketimeschemagroup";

BlobContainerClient blobContainerClient = new(storageAccountConnectionString, "myeventhub-blobs");

var processor = new EventProcessorClient(
    blobContainerClient,
    EventHubConsumerClient.DefaultConsumerGroupName,
    eventHubsConnectionString,
    hubName);

processor.ProcessEventAsync += OnProcessEventAsync;
processor.ProcessErrorAsync += OnProcessErrorAsync;

await processor.StartProcessingAsync();

await Task.Delay(TimeSpan.FromSeconds(30));

await processor.StopProcessingAsync();

//Task OnProcessEventAsync(ProcessEventArgs args)
//{
//    Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(args.Data.Body.ToArray()));
//    return Task.CompletedTask;
//}

Task OnProcessErrorAsync(ProcessErrorEventArgs args)
{
    Console.WriteLine($"\tPartition '{args.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
    Console.WriteLine(args.Exception.Message);
    return Task.CompletedTask;
}

async Task OnProcessEventAsync(ProcessEventArgs args)
{
    // Create a schema registry client that you can use to serialize and validate data.  
    var schemaRegistryClient = new SchemaRegistryClient(schemaRegistryEndpoint, new DefaultAzureCredential());

    // Create an Avro object serializer using the Schema Registry client object. 
    var serializer = new SchemaRegistryAvroSerializer(schemaRegistryClient, schemaGroup, new SchemaRegistryAvroSerializerOptions { AutoRegisterSchemas = true });

    // Deserialized data in the received event using the schema 
    Order sampleOrder = (Order)await serializer.DeserializeAsync(args.Data, typeof(Order));

    // Print the received event
    Console.WriteLine($"Received order with ID: {sampleOrder.id}, amount: {sampleOrder.amount}, description: {sampleOrder.description}");

    await args.UpdateCheckpointAsync(args.CancellationToken);
}