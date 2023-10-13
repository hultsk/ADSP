using ADSP.ConsoleApplication.DevOps;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSP.ConsoleApplication.DevOps
{
   public class AzureDevOpsConnectionManager
   {
      private static VssConnection? m_connection;

      public static VssConnection GetConnection(string? personalAccessToken, string? organizationUrl)
      {
         if (m_connection == null)
         {
            if (personalAccessToken == null || organizationUrl == null)
            {
               throw new ArgumentException("Both personalAccessToken and organizationUrl must be provided.");
            }
            m_connection = new VssConnection(new Uri(organizationUrl), new VssBasicCredential(string.Empty, personalAccessToken));
            m_connection.ConnectAsync().Wait();
         }
         return m_connection;
      }

      public static VssConnection GetConnection()
      {
         if (m_connection != null)
         {
            return m_connection;
         }
         else
         {
            throw new InvalidOperationException("No existing Azure DevOps connection found.");
         }
      }

      public static void ResetConnection()
      {
         m_connection = null;
      }
   }
}