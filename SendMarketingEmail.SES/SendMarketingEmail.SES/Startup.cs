using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendMarketingEmail.SES.Interfaces;
using SendMarketingEmail.SES.Models;
using SendMarketingEmail.SES.Services;

namespace SendMarketingEmail.SES
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<AwsEmailServiceOptions>(Configuration.GetSection("AwsEmailServiceOptions"));
      services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
      services.AddAWSService<IAmazonSimpleEmailService>();      
      services.AddHostedService<SendEmailService>();
      services.AddTransient<IAwsEmailService, AwsEmailService>();
      services.AddControllers();
      
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
