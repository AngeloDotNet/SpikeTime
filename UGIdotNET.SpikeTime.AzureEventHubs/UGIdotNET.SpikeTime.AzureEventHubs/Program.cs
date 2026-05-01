using Azure.Data.SchemaRegistry;
using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using com.azure.schemaregistry.samples;
using Microsoft.Azure.Data.SchemaRegistry.ApacheAvro;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var eventHubsConnectionString = configuration.GetConnectionString("AzureEventHubs");
const string hubName = "ordershub";

// Schema Registry endpoint 
const string schemaRegistryEndpoint = "spiketime-eventhub.servicebus.windows.net";

// name of the consumer group   
const string schemaGroup = "spiketimeschemagroup";

await using EventHubProducerClient producerClient = new(
    eventHubsConnectionString,
    hubName);

using EventDataBatch eventDataBatch = await producerClient.CreateBatchAsync();

//for (int i = 0; i < 3; i++)
//{
//    var eventData = new EventData($"Spike time - event #{i + 1}");
//    if (!eventDataBatch.TryAdd(eventData))
//    {
//        throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
//    }
//}

var schemaRegistryClient = new SchemaRegistryClient(schemaRegistryEndpoint, new DefaultAzureCredential());

// Create an Avro object serializer using the Schema Registry client object. 
var serializer = new SchemaRegistryAvroSerializer(schemaRegistryClient, schemaGroup, new SchemaRegistryAvroSerializerOptions { AutoRegisterSchemas = true });

// Create a new order object using the generated type/class 'Order'. 
var sampleOrder = new Order { id = "1234", amount = 45.29, description = "First sample order." };
EventData eventData = (EventData)await serializer.SerializeAsync(sampleOrder, messageType: typeof(EventData));

// Create a batch of events 
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

// Add the event data to the event batch. 
eventBatch.TryAdd(eventData);

// Send the batch of events to the event hub. 
await producerClient.SendAsync(eventBatch);
Console.WriteLine("A batch of 1 order has been published.");

//await producerClient.SendAsync(eventDataBatch);

//Console.WriteLine($"A batch of 3 events has been published on hub {hubName}.");
//Console.ReadLine();
