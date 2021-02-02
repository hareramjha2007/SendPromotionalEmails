using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SendMarketingEmail.SES.Interfaces;
using SendMarketingEmail.SES.Models;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SendMarketingEmail.SES.Services
{
  public class AwsEmailService : IAwsEmailService
  {
    private readonly ILogger<AwsEmailService> _logger;
    private readonly IAmazonSimpleEmailService _emailService;
    private readonly IOptions<AwsEmailServiceOptions> _emailOptions;

    public AwsEmailService(IAmazonSimpleEmailService emailService, IOptions<AwsEmailServiceOptions> emailOptions,
        ILogger<AwsEmailService> logger)
    {
      _logger = logger;
      _emailService = emailService;
      _emailOptions = emailOptions;
    }

    public async Task<bool> SendEmailAsync(string toAddress, string body, bool isHtmlBody, CancellationToken cancellationToken)
    {
      var emailMessage = BuildEmailHeaders(toAddress);
      var emailBody = BuildEmailBody(body, isHtmlBody);
      emailMessage.Body = emailBody.ToMessageBody();
      await SendEmailAsync(emailMessage, cancellationToken);
      return true;
    }


    #region helpers

    private static BodyBuilder BuildEmailBody(string body, bool isHtmlBody = true)
    {
      var bodyBuilder = new BodyBuilder();
      if (isHtmlBody)
      {
        bodyBuilder.HtmlBody = body;
      }
      else
      {
        bodyBuilder.TextBody = body;
      }
      return bodyBuilder;
    }

    private MimeMessage BuildEmailHeaders(string to)
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(_emailOptions.Value.SenderName, _emailOptions.Value.SenderEmail));
      message.To.Add(new MailboxAddress(string.Empty, to));
      message.Subject = _emailOptions.Value.EmailSubject;
      return message;
    }

    private async Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken)
    {
      try
      {
        using (var memoryStream = new MemoryStream())
        {
          await message.WriteToAsync(memoryStream);
          var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(memoryStream) };

          var response = await _emailService.SendRawEmailAsync(sendRequest, cancellationToken);
          if (response.HttpStatusCode == HttpStatusCode.OK)
          {
            _logger.LogInformation($"The email with message Id {response.MessageId} sent successfully to {message.To} on {DateTime.UtcNow:O}");
          }
          else
          {
            _logger.LogError($"Failed to send email with message Id {response.MessageId} to {message.To} on {DateTime.UtcNow:O} due to {response.HttpStatusCode}.");
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"An error occured while sending email to {message.To}");
      }
    }
    #endregion
  }
}
