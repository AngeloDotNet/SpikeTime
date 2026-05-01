using AWS.Messaging.Publishers.EventBridge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var awsAccessKey = configuration["Aws:AccessKey"]!;
var awsSecretKey = configuration["Aws:Secret"]!;

var services = new ServiceCollection();

services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions
{
    Credentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKey, awsSecretKey),
    Region = Amazon.RegionEndpoint.EUWest1
});

services.AddAWSMessageBus(builder =>
{
    builder.AddEventBridgePublisher<MyMessage>("<event bus arn>");
});

var provider = services.BuildServiceProvider();

var eventBridgePublisher = provider.GetRequiredService<IEventBridgePublisher>();

int i = 0;

do
{
    try
    {
        var message = new MyMessage($"Ciao Spike time! {i} - {DateTime.Now}");
        var response = await eventBridgePublisher.PublishAsync(message);

        Console.WriteLine($"Message {response.MessageId} published!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex}");
    }
    finally
    {
        i++;
    }
} while (i < 5);

public record MyMessage(string Text);