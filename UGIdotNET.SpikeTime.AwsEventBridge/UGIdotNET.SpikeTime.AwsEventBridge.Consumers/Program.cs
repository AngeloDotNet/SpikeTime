using AWS.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("[Consumers] Hello, World!");

var builder = Host.CreateApplicationBuilder();

builder.Configuration
    .AddUserSecrets<Program>()
    .Build();

var awsAccessKey = builder.Configuration["Aws:AccessKey"]!;
var awsSecretKey = builder.Configuration["Aws:Secret"]!;

builder.Services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions
{
    Credentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKey, awsSecretKey),
    Region = Amazon.RegionEndpoint.EUWest1
});

builder.Services.AddAWSMessageBus(builder =>
{
    builder.AddSQSPoller<MyMessage>("<sqs arn>");

    builder.AddMessageHandler<MyMessageHandler, MyMessage>();
});

var host = builder.Build();
await host.RunAsync();

public record MyMessage(string Text);

class MyMessageHandler : IMessageHandler<MyMessage>
{
    public Task<MessageProcessStatus> HandleAsync(MessageEnvelope<MyMessage> messageEnvelope, CancellationToken token = default)
    {
        Console.WriteLine($"Envelope ID: {messageEnvelope.Id}");
        Console.WriteLine($"Message text: {messageEnvelope.Message.Text}");

        var status = MessageProcessStatus.Success();
        return Task.FromResult(status);
    }
}