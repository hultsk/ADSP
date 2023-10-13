using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;

namespace ADSP.ConsoleApplication.DevOps
{
   public class AzureDevOpsProvider
   {
      private readonly VssConnection m_connection;
      private readonly string m_projectName;
      private WorkItemTrackingHttpClient? m_workItemClient;

      public AzureDevOpsProvider(VssConnection connection, string? projectName)
      {
         m_projectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
         m_connection = connection ?? throw new ArgumentNullException(nameof(connection));
      }

      private WorkItemTrackingHttpClient WorkItemClient => m_workItemClient ??= m_connection.GetClient<WorkItemTrackingHttpClient>();

      public async Task<List<WorkItem>> GetWorkItemsForIterationAsync(string iterationPath)
      {
         var wiqlQuery = $@"
                   SELECT [System.Id]
                   FROM WorkItems
                   WHERE [System.TeamProject] = '{m_projectName}'
                   AND [System.IterationPath] = '{iterationPath}'
                   AND [System.AreaPath] UNDER 'iCore v3\ICPS\'
                   AND ([System.WorkItemType] = 'User Story' OR [System.WorkItemType] = 'Bug')
                   ORDER BY [System.Id]";

         var wiql = new Wiql
         {
            Query = wiqlQuery
         };

         return await GetWorkItemsByWiqlAsync(wiql);
      }

      private async Task<List<WorkItem>> GetWorkItemsByWiqlAsync(Wiql wiql)
      {
         var result = await WorkItemClient.QueryByWiqlAsync(wiql);
         if (result.WorkItems.Any())
         {
            var ids = result.WorkItems.Select(w => w.Id).ToArray();
            return await GetWorkItemsAsync(ids);
         }
         return new List<WorkItem>();
      }

      private async Task<List<WorkItem>> GetWorkItemsAsync(IEnumerable<int> ids)
      {
         return await WorkItemClient.GetWorkItemsAsync(ids);
      }
   }
}