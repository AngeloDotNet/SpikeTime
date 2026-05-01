using Azure.Core.Serialization;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

//await SendToEventGridTopic(configuration);
await SendToEventGridDomain(configuration);

async Task SendToEventGridDomain(IConfigurationRoot configuration)
{
    var eventGridDomainEndpoint = configuration["AzureEventGrid:DomainEndpoint"]!;
    var eventGridDomainKey = configuration["AzureEventGrid:DomainKey"]!;

    EventGridPublisherClient client = new EventGridPublisherClient(
        new(eventGridDomainEndpoint),
        new Azure.AzureKeyCredential(eventGridDomainKey));

    var serializer = new JsonObjectSerializer(
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    EventGridEvent[] eventList = [
        new EventGridEvent(
            "SpikeTimeEventGridSubject",
            typeof(EventPayload).FullName,
            "1.0",
            new EventPayload{ Text = $"Ciao da spike time! {DateTime.Now}" }
        ){ Topic = "topic1" },
        new EventGridEvent(
            "SpikeTimeEventGridSubject",
            typeof(EventPayload).FullName,
            "1.0",
            serializer.Serialize(new EventPayload { Text = $"Evento serializzato {DateTime.Now}" })
        ){ Topic = "topic2" },
    ];

    var response = await client.SendEventsAsync(eventList);

    Console.WriteLine("Eventi inviati!");
}

async Task SendToEventGridTopic(IConfiguration configuration)
{
    var eventGridTopicEndpoint = configuration["AzureEventGrid:Endpoint"]!;
    var eventGridTopicKey = configuration["AzureEventGrid:Key"]!;

    EventGridPublisherClient client = new EventGridPublisherClient(
        new(eventGridTopicEndpoint),
        new Azure.AzureKeyCredential(eventGridTopicKey));

    var serializer = new JsonObjectSerializer(
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    EventGridEvent[] eventList = [
        new EventGridEvent(
            "SpikeTimeEventGridSubject",
            typeof(EventPayload).FullName,
            "1.0",
            new EventPayload{ Text = $"Ciao da spike time! {DateTime.Now}" }
        ),
        new EventGridEvent(
            "SpikeTimeEventGridSubject",
            typeof(EventPayload).FullName,
            "1.0",
            serializer.Serialize(new EventPayload { Text = $"Evento serializzato {DateTime.Now}" })
        ),
    ];

    var response = await client.SendEventsAsync(eventList);

    Console.WriteLine("Eventi inviati!");
}


record EventPayload
{
    public string Text { get; init; } = string.Empty;
}