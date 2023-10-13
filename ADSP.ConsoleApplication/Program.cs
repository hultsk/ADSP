using ADSP.ConsoleApplication.DevOps;
using ADSP.ConsoleApplication.Utilities;
using Microsoft.Extensions.Configuration;

namespace ADSP.ConsoleApplication
{
   internal class Program
   {
      private static async Task Main(string[] args)
      {
         IConfiguration configuration = InitializeConfiguration();

         string pat = configuration["ApplicationSettings:PersonalAccessToken"] ?? throw new ArgumentNullException("PAT not provided in configuration");
         string organizationUrl = configuration["ApplicationSettings:OrganizationUrl"] ?? throw new ArgumentNullException("Organization URL not provided in configuration");
         string project = configuration["ApplicationSettings:Project"] ?? throw new ArgumentNullException("Project not provided in configuration");
         string iteration = configuration["ApplicationSettings:Iteration"] ?? throw new ArgumentNullException("Iteration not provided in configuration");

         var connection = AzureDevOpsConnectionManager.GetConnection(pat, organizationUrl);
         var provider = new AzureDevOpsProvider(connection, project);

         var workItems = await provider.GetWorkItemsForIterationAsync(iteration);

         Console.WriteLine(workItems);

        }

      private static IConfiguration InitializeConfiguration()
      {
         var configBuilder = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

         return configBuilder.Build();
      }
   }
}