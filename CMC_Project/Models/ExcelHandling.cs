using System;
using System.IO;
//공통 NPOI
using NPOI;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
//표준 xls 버전 excel 시트
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
//확장 xlsx 버전 excel 시트
using NPOI.XSSF;
using NPOI.XSSF.UserModel;

namespace SetUnitPriceByExcel
{
    class ExcelHandling
    {
        // Sheet로 부터 Row를 취득, 생성하기
        public static IRow GetRow(ISheet sheet, int rownum)
        {
            var row = sheet.GetRow(rownum);
            if (row == null)
            {
                row = sheet.CreateRow(rownum);
            }
            return row;
        }
        // Row로 부터 Cell를 취득, 생성하기
        public static ICell GetCell(IRow row, int cellnum)
        {
            var cell = row.GetCell(cellnum);
            if (cell == null)
            {
                cell = row.CreateCell(cellnum);
            }
            return cell;
        }
        public static ICell GetCell(ISheet sheet, int rownum, int cellnum)
        {
            var row = GetRow(sheet, rownum);
            return GetCell(row, cellnum);
        }
        // Workbook 읽어드리기
        static public IWorkbook GetWorkbook(string filename, string version)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                //표준 xls 버전
                if (".xls".Equals(version))
                {
                    return new HSSFWorkbook(stream);
                }
                //확장 xlsx 버전
                else if (".xlsx".Equals(version))
                {
                    return new XSSFWorkbook(stream);
                }
                throw new NotSupportedException();
            }
        }

        //작업 후, workbook 저장
        static public void WriteExcel(IWorkbook workbook, string filepath)
        {
            using (var file = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite))
            {
                workbook.Write(file);
            }
        }
    }
}