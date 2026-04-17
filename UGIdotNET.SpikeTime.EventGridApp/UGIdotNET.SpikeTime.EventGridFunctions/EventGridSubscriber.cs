// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UGIdotNET.SpikeTime.EventGridFunctions;

public class EventGridSubscriber
{
    private readonly ILogger<EventGridSubscriber> _logger;

    public EventGridSubscriber(ILogger<EventGridSubscriber> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EventGridSubscriber))]
    public void Run([EventGridTrigger] CloudEvent cloudEvent)
    {
        _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
    }

    [Function(nameof(DomainTopic1Subscriber))]
    public void DomainTopic1Subscriber([EventGridTrigger] CloudEvent cloudEvent)
    {
        _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
    }

    [Function(nameof(DomainTopic2Subscriber))]
    public void DomainTopic2Subscriber([EventGridTrigger] CloudEvent cloudEvent)
    {
        _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
    }

    [Function(nameof(DomainSubscriber))]
    public void DomainSubscriber([EventGridTrigger] CloudEvent cloudEvent)
    {
        _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
    }
}