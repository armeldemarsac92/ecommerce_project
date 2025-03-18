using MassTransit;
using Microsoft.Extensions.Logging;
using Tdev702.Auth.SDK.Service;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Messaging;

namespace Tdev702.Auth.SDK.Consumer;

public class TwoFactorCodeConsumer : IConsumer<TwoFactorCodeTask>
{
    private readonly ICodeStateService _codeStateService;
    private readonly ILogger<TwoFactorCodeConsumer> _logger;

    public TwoFactorCodeConsumer(
        ILogger<TwoFactorCodeConsumer> logger, 
        ICodeStateService codeStateService)
    {
        _logger = logger;
        _codeStateService = codeStateService;
    }

    public Task Consume(ConsumeContext<TwoFactorCodeTask> context)
    {
        var message = context.Message;
        
        if (string.IsNullOrEmpty(message.Code))
        {
            _logger.LogError("Received an empty code for email: {email}", message.Email);
            throw new EmptyCodeException($"Empty code received for email: {message.Email}");
        }
        _logger.LogInformation("Received code {code} for email: {email}", message.Code, message.Email);
        _codeStateService.SetGeneratedCode(message.Email, message.Code);
        
        return Task.CompletedTask;
    }
}