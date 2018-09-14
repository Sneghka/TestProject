using ClosedXML.Excel;
using Cwc.BaseData;
using System;
using System.Collections.Generic;

namespace Specflow.Automation.Backend.Objects
{    
    public class ServiceOrderImportFormatAContainer
    {
        private XLWorkbook workBook;
        private IXLWorksheet workSheet;        
        private int rowNumber = 1;
        public DateTime ServiceDate { get; set; }
        public string CompanyName { get; set; }
        public int CompanyNumber { get; set; }
        public string ServiceType { get; set; }
        public static string FolderPath { get; set; }
        public static string FileName { get; set; }
        public static Product Sar500Product { get; set; }
        public static Product Sar100Product { get; set; }
        public static Product Sar50Product { get; set; }
        public static Product Sar10Product { get; set; }
        public static Product Usd100Product { get; set; }
        public List<ServiceOrderImportFormatARow> FormatARowList { get; set; }

        public ServiceOrderImportFormatAContainer()
        {
            workBook = new XLWorkbook { RightToLeft = true };
            workSheet = workBook.Worksheets.Add("Orders List");
            FormatARowList = new List<ServiceOrderImportFormatARow>();
        }        
        
        public ServiceOrderImportFormatAContainer Compose()
        {
            workSheet.Cell("C3").Value = ServiceDate;
            workSheet.Cell("I3").Value = CompanyName;
            workSheet.Cell("J3").Value = CompanyNumber;
            workSheet.Cell("J2").Value = ServiceType;

            foreach (var row in FormatARowList)
            {
                var rowCount = rowNumber + 6;   // orderlines are configured starting from row 7 in Excel file
                workSheet.Cell($"A{rowCount}").Value = rowNumber;
                workSheet.Cell($"A{rowCount}").DataType = XLCellValues.Text;
                workSheet.Cell($"B{rowCount}").Value = row.LocationName;
                workSheet.Cell($"C{rowCount}").Value = row.LocationCode;
                workSheet.Cell($"E{rowCount}").Value = row.Sar500;
                workSheet.Cell($"E{rowCount}").DataType = XLCellValues.Text;
                workSheet.Cell($"F{rowCount}").Value = row.Sar100;
                workSheet.Cell($"F{rowCount}").DataType = XLCellValues.Text;
                workSheet.Cell($"G{rowCount}").Value = row.Sar50;
                workSheet.Cell($"G{rowCount}").DataType = XLCellValues.Text;
                workSheet.Cell($"H{rowCount}").Value = row.Sar10;
                workSheet.Cell($"H{rowCount}").DataType = XLCellValues.Text;
                workSheet.Cell($"I{rowCount}").Value = row.Usd100;
                workSheet.Cell($"I{rowCount}").DataType = XLCellValues.Text;
                rowNumber++;
            }

            return this;
        }

        public void Save()
        {
            workBook.SaveAs($"{FolderPath}\\{FileName}");
        }
    }
}
