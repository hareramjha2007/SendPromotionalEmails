namespace SendMarketingEmail.SES.Models
{
  public class AwsEmailServiceOptions
  {
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string EmailAddressFile { get; set; }
    public string EmailSubject { get; set; }
    public string EmailTemplate { get; set; }
  }
}
