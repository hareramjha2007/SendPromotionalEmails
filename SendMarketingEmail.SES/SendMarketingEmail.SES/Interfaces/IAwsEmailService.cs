using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SendMarketingEmail.SES.Interfaces
{
  public interface IAwsEmailService
  {
    Task<bool> SendEmailAsync(string toAddress, string body, bool isHtmlBody, CancellationToken cancellationToken);
  }
}
