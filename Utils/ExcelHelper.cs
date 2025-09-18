using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

namespace FionaAutomation.Utils
{
    public class TestCase
    {
        public string TestCaseID { get; set; }
        public string TestCaseName { get; set; }
        public string MethodName { get; set; }
        public bool Execute { get; set; }
        public string Result { get; set; }
    }

    public class ExcelHelper
    {
        private readonly string _filePath;
        private readonly IXLWorkbook _workbook;
        private readonly IXLWorksheet _worksheet;

        public ExcelHelper(string relativeFilePath)
        {
            if (string.IsNullOrWhiteSpace(relativeFilePath))
                throw new ArgumentNullException(nameof(relativeFilePath));

            
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
            _filePath = Path.Combine(projectRoot, relativeFilePath);

            if (!File.Exists(_filePath))
                throw new FileNotFoundException($"Excel file not found at {_filePath}");

            _workbook = new XLWorkbook(_filePath);
            _worksheet = _workbook.Worksheet(1); // First sheet
        }

        public List<TestCase> GetTests()
        {
            var tests = new List<TestCase>();
            var rows = _worksheet.RangeUsed().RowsUsed();

            foreach (var row in rows.Skip(1)) // Skip header
            {
                var testCase = new TestCase
                {
                    TestCaseID = row.Cell(1).GetString(),
                    TestCaseName = row.Cell(2).GetString(),
                    MethodName = row.Cell(4).GetString(),
                    Execute = row.Cell(3).GetString().Equals("TRUE", StringComparison.OrdinalIgnoreCase),
                    Result = row.Cell(5).GetString()
                };
                tests.Add(testCase);
            }

            return tests;
        }

        public void WriteResult(string testCaseID, string result)
        {
            var rows = _worksheet.RangeUsed().RowsUsed();

            foreach (var row in rows.Skip(1))
            {
                var cellValue = row.Cell(1).GetString().Trim();
                if (cellValue.Equals(testCaseID, StringComparison.OrdinalIgnoreCase))
                {
                    row.Cell(5).Value = result; // Column E: Result
                    Console.WriteLine($"Result written for {testCaseID}: {result}");
                    break;
                }
            }

         
            _workbook.Save();
        }
    }
}
