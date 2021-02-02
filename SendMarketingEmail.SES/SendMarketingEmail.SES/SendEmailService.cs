using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendMarketingEmail.SES.Interfaces;
using SendMarketingEmail.SES.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SendMarketingEmail.SES
{
  public class SendEmailService : BackgroundService
  {
    private readonly IOptions<AwsEmailServiceOptions> _emailOptions;
    private readonly IAwsEmailService _awsEmailService;
    private readonly ILogger<SendEmailService> _logger;

    public SendEmailService(IOptions<AwsEmailServiceOptions> emailOptions, IAwsEmailService awsEmailService, ILogger<SendEmailService> logger)
    {
      _emailOptions = emailOptions;
      _awsEmailService = awsEmailService;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      List<string> emailAddresses = new List<string>(await File.ReadAllLinesAsync(_emailOptions.Value.EmailAddressFile));
      string emailBodyTemplate = await File.ReadAllTextAsync(_emailOptions.Value.EmailTemplate);

      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          foreach (var emailAddress in emailAddresses)
          {
            var currentIndex = emailAddresses.IndexOf(emailAddress);
            _logger.LogInformation($"{currentIndex + 1}/{emailAddresses.Count} : Sending email to {emailAddress}");
            await _awsEmailService.SendEmailAsync(emailAddress, emailBodyTemplate, true, stoppingToken);
          }
          _logger.LogInformation("Sent email to all recepients.");
          _logger.LogInformation("If you want to resend or send to new list of recepients then restart this service again.");
          break;
        }
        catch (Exception) when (stoppingToken.IsCancellationRequested)
        {
          break;
        }
        catch(Exception ex)
        {
          _logger.LogError(ex, "An error occured while receiving the mesages.");
        }
      }
    }
  }
}
