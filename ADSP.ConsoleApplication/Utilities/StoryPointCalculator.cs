using ADSP.ConsoleApplication.DevOps;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace ADSP.ConsoleApplication.Utilities
{
   public static class StoryPointCalculator
   {
      public static double CalculateTotalStoryPoints(List<WorkItem> workItems)
      {
         return workItems.Sum(GetStoryPointsFromWorkItem);
      }

      public static double CalculateActiveStoryPoints(List<WorkItem> workItems)
      {
         return workItems
             .Where(w => w.Fields.TryGetValue(FieldNames.State, out var state) && state.ToString() == "Active")
             .Sum(GetStoryPointsFromWorkItem);
      }

      public static double CalculateDifferenceInStoryPoints(List<WorkItem> workItems)
      {
         return CalculateTotalStoryPoints(workItems) - CalculateActiveStoryPoints(workItems);
      }

      private static double GetStoryPointsFromWorkItem(WorkItem workItem)
      {
         return workItem.Fields.TryGetValue(FieldNames.StoryPoints, out var storyPointsValue)
             ? Convert.ToDouble(storyPointsValue)
             : 0;
      }
   }
}