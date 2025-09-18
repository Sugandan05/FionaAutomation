using ClosedXML.Excel;
using FionaAutomation.Models;
using System.Collections.Generic;

namespace FionaAutomation.Utils
{
    public static class ExcelDataHelper
    {
        public static List<CreateRequestData> GetCreateRequestData(string filePath, string sheetName)
        {
            var requestDataList = new List<CreateRequestData>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(sheetName);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header row

                foreach (var row in rows)
                {
                    var requestData = new CreateRequestData
                    {
                        PaymentDueBy = row.Cell(1).GetString(),
                        PaymentType = row.Cell(2).GetString(),
                        PaymentRegion = row.Cell(3).GetString(),
                        AccountType = row.Cell(4).GetString(),
                        AccountName = row.Cell(5).GetString(),
                        BeneficiaryName = row.Cell(6).GetString(),
                        SortCode = row.Cell(7).GetString(),
                        AccountNumber = row.Cell(8).GetString(),
                        IBAN = row.Cell(9).GetString(),
                        BIC = row.Cell(10).GetString(),
                        NetValue = row.Cell(11).GetString(),
                         VATApplicable = row.Cell(12).GetString(),
                        VATAmount = row.Cell(13).GetString(),
                       // JournalAccount = row.Cell(14).GetString(),
                        Currency = row.Cell(14).GetString(),
                        Entity = row.Cell(15).GetString(),
                        NominalAccount = row.Cell(16).GetString(),
                        CostCentre = row.Cell(17).GetString(),
                        Description = row.Cell(18).GetString(),
                        AdditionalComments = row.Cell(20).GetString(),
                        SupportingDocumentPath = row.Cell(21).GetString()
                    };

                    requestDataList.Add(requestData);
                }
            }

            return requestDataList;
        }
    }
}
