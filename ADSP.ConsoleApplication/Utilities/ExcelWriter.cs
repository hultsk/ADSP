using OfficeOpenXml;

namespace ADSP.ConsoleApplication.Utilities
{
   public static class ExcelWriter
   {
      private const string FilePath = "Atlas Storypoints .xlsx";
      private const string SheetName = "StoryPointsData";

      public static void WriteToExcel(string iterationPath, double totalSP, double completedSP)
      {
         FileInfo file = new FileInfo(FilePath);

         using (ExcelPackage package = new ExcelPackage(file))
         {
            ExcelWorksheet worksheet;

            if (!file.Exists)
            {
               worksheet = package.Workbook.Worksheets.Add(SheetName);

               // Set header
               worksheet.Cells[1, 1].Value = "Iteration/Sprint";
               worksheet.Cells[1, 2].Value = "Total SP";
               worksheet.Cells[1, 3].Value = "Completed SP";

               worksheet.Cells[2, 1].Value = iterationPath;
               worksheet.Cells[2, 2].Value = totalSP;
               worksheet.Cells[2, 3].Value = completedSP;
            }
            else
            {
               worksheet = package.Workbook.Worksheets[SheetName];

               int lastRow = worksheet.Dimension.End.Row + 1;

               worksheet.Cells[lastRow, 1].Value = iterationPath;
               worksheet.Cells[lastRow, 2].Value = totalSP;
               worksheet.Cells[lastRow, 3].Value = completedSP;
            }

            package.Save();
         }
      }
   }
}