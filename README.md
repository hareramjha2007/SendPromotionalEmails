# Send Promotional Emails
If you want to send promotional/business emails to millions of people at one go then this solution will help to do that. There are some third party providers like SendGrid offers such paid services. But with this solution you just need to pay small amount to the Amazon AWS. Amazon charge is very low and you pay only for what you have used.
I have used the AWS Simple Email Service(SES) in this solution. You can even try it for free if you want to try it out.

Give a try with these easy steps:
1. Create an email template. You can create your own template by your own or you can use third party editors like www.designedwithbee.com , sample template is attached to the soltuion. 
2. Create Amazon AWS service account for free or paid.
3. Go to https://console.aws.amazon.com/ses or search for SES in service list.
4. Create Domain and verify or Create email and verify.
5. Open this solution and change the configuration file "appsettings.Development.json" if you are running in debug mode. Or else "appsettings.json" for release mode.
6. Build and run the solution.

This will run as console application. You can notice the log for success or failure.

If the above try succeeded then you are ready to go with actual recipients. You can create your file with email list and store in EmailAddresses directory. Please Node: to send emails to actual recipients, you should not have your SES service as Sandbox.
