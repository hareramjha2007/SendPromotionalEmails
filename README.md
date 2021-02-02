# SendPromotionalEmails
If you want to send promotional/business emails to millions of people at one go then this solution will help to do that. There are some third party providers like SendGrid offers such paid services. But with this solution you just need to pay small amount to the Amazon AWS. Amazon charge is very low and you pay only for what you have used.
I have used the AWS Simple Email Service(SES) in this solution. You can even try it for free if you want to try it out.

These are following steps you need to follow:
1. Create an email template. You can create your template by your own or you can use third party editors like www.designedwithbee.com
2. Create Amazon AWS service account for free or paid.
3. Go to https://console.aws.amazon.com/ses or search for SES in service list.
4. Create Domain and verify or Create email and verify.
5. Open this solution and change the configuration file "appsettings.Development.json" if you are running in debug mode. Or else "appsettings.json" for release mode.
6. Build and run the solution.
