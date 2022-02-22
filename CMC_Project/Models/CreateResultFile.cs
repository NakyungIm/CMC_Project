using System;
using System.IO;
using System.Collections.Generic;

namespace SetUnitPriceByExcel
{
    class CreateResultFile
    {
        public static void Create()
        {
            foreach(KeyValuePair<string, List<Data>> dic in Data.Dic)
            {
                var workbook = ExcelHandling.GetWorkbook("입찰내역.xls", ".xls");   //입찰내역 양식 불러오기
                var sheet = workbook.GetSheetAt(0);
                string resultPath;   //최종 입찰내역 파일 생성 경로
                string path;

                //입찰내역 항목 작성
                for(int i=0; i<dic.Value.Count; i++)
                {
                    //일반 항목인 경우 단가 조정 후, 소수점 첫째 자리 아래로 절사
                    if (dic.Value[i].Item.Equals("일반"))
                    {
                        ExcelHandling.GetCell(sheet, i, 22).SetCellValue((double)(dic.Value[i].PriceScore));  //세부 점수 (0 or 100)
                        ExcelHandling.GetCell(sheet, i, 23).SetCellValue((double)(dic.Value[i].Score));  //단가 점수(세부 점수 * 가중치)
                    }
                    var materialUnit = (double)(dic.Value[i].MaterialUnit);
                    var laborUnit = (double)(dic.Value[i].LaborUnit);
                    var expenseUnit = (double)(dic.Value[i].ExpenseUnit);
                    var material = (double)(dic.Value[i].Material);
                    var labor = (double)(dic.Value[i].Labor);
                    var expense = (double)(dic.Value[i].Expense);
                    var unitpricesum = (double)(dic.Value[i].UnitPriceSum);
                    var pricesum = (double)(dic.Value[i].PriceSum);

                    ExcelHandling.GetCell(sheet, i+1, 1).SetCellValue((i+1) * 100);  //순번
                    ExcelHandling.GetCell(sheet, i+1, 3).SetCellValue(dic.Value[i].Name);  //공종명
                    ExcelHandling.GetCell(sheet, i+1, 4).SetCellValue(dic.Value[i].Standard);  //규격
                    ExcelHandling.GetCell(sheet, i+1, 5).SetCellValue(dic.Value[i].Unit);  //단위
                    ExcelHandling.GetCell(sheet, i+1, 6).SetCellValue((double)(dic.Value[i].Quantity));  //수량
                    ExcelHandling.GetCell(sheet, i+1, 7).SetCellValue(materialUnit);  //재료비단가
                    ExcelHandling.GetCell(sheet, i+1, 8).SetCellValue(laborUnit);  //노무비단가
                    ExcelHandling.GetCell(sheet, i+1, 9).SetCellValue(expenseUnit);  //경비단가
                    ExcelHandling.GetCell(sheet, i+1, 10).SetCellValue(unitpricesum);  //합계단가
                    ExcelHandling.GetCell(sheet, i+1, 11).SetCellValue(material);  //재료비
                    ExcelHandling.GetCell(sheet, i+1, 12).SetCellValue(labor);  //노무비
                    ExcelHandling.GetCell(sheet, i+1, 13).SetCellValue(expense);  //경비
                    ExcelHandling.GetCell(sheet, i+1, 14).SetCellValue(pricesum);  //합 계
                    ExcelHandling.GetCell(sheet, i+1, 18).SetCellValue(dic.Value[i].Code);  //원설계코드

                    if (dic.Value[i].Item.Equals("표준시장단가"))
                    {
                        ExcelHandling.GetCell(sheet, i+1, 16).SetCellValue(dic.Value[i].Item);  //세부공종구분
                        ExcelHandling.GetCell(sheet, i+1, 17).SetCellValue(dic.Value[i].Code);  //표준시장단가코드
                    }
                    else
                    {
                        ExcelHandling.GetCell(sheet, i+1, 15).SetCellValue(dic.Value[i].Item);  //비고
                    }
                }

                //세부 공사별로 입찰내역 파일 저장
                resultPath = "입찰내역_" + Data.ConstructionNums[dic.Key] + ".xls";
                path = Path.Combine(Data.desktop_path, resultPath);
                ExcelHandling.WriteExcel(workbook, path);
            }
        }
    }
}