using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;

namespace SetUnitPriceByExcel
{
    class Setting
    {
        static XDocument docBID;   //공내역BID -> xml
        static IEnumerable<XElement> eleBID;

        static void GetConstructionNum()    //세부 공사별 번호 저장
        {
            //<T5> 요소의 자식 요소에 위치한 세부공사별 번호 저장
            var constNums = from t in eleBID
                            where t.Name == "T5"
                            select t;
            foreach (var num in constNums)
            {
                string index = string.Concat(num.Element("C1").Value);
                string construction = string.Concat(num.Element("C2").Value);
                if (Data.ConstructionNums.ContainsValue(construction))
                    construction += "2";
                Data.ConstructionNums.Add(index, construction);
            }
        }

        static void AddConstructionList()
        {
            //Data.Dic에 세부공사별 리스트 추가
            foreach (KeyValuePair<string, string> constNum in Data.ConstructionNums)
            {
                Data.Dic.Add(constNum.Key, new List<Data>());
            }
        }

        static void GetDataFromBID()
        {
            //공내역 xml 파일 읽어들여 데이터 객체에 저장
            var works = from work in eleBID
                        where work.Name == "T6"
                        select new Data()
                        {
                            Item = GetItem(work),
                            ConstructionNum = string.Concat(work.Element("C1").Value),
                            WorkNum = string.Concat(work.Element("C2").Value),
                            DetailWorkNum = string.Concat(work.Element("C3").Value),
                            Code = string.Concat(work.Element("C24").Value),
                            Name = string.Concat(work.Element("C9").Value),
                            Standard = string.Concat(work.Element("C10").Value),
                            Unit = string.Concat(work.Element("C11").Value),
                            Quantity = Convert.ToDecimal(work.Element("C13").Value),
                            MaterialUnit = Convert.ToDecimal(work.Element("C15").Value),
                            LaborUnit = Convert.ToDecimal(work.Element("C16").Value),
                            ExpenseUnit = Convert.ToDecimal(work.Element("C17").Value),
                        };
            //항목에 해당하는 세부공사의 리스트에 객체 추가
            foreach (var work in works)
            {
                Data.Dic[work.ConstructionNum].Add(work);
            }
        }

        static string GetItem(XElement bid)
        {
            string item = null;
            //해당 공종이 일반, 표준시장단가 및 공종(입력불가) 항목인 경우
            if (string.Concat(bid.Element("C6").Value) == "0")
            {
                if (string.Concat(bid.Element("C4").Value) == "S")
                {
                    if (string.Concat(bid.Element("C5").Value) == "0")
                        item = "표준시장단가";
                    else
                        item = "일반";
                }
                else if (string.Concat(bid.Element("C4").Value) == "G")
                    item = "공종(입력불가)";
            }
            //해당 공종이 무대(입력불가)인 경우
            else if (string.Concat(bid.Element("C6").Value) == "1")
                item = "무대(입력불가)";
            //해당 공종이 관급자재인 경우
            else if (string.Concat(bid.Element("C6").Value) == "2")
                item = "관급자재";
            //해당 공종이 관급자재인 경우
            else if (string.Concat(bid.Element("C6").Value) == "3")
                item = "관급공종";
            //해당 공종이 PS인 경우
            else if (string.Concat(bid.Element("C6").Value) == "4")
                item = "PS";
            //해당 공종이 제요율적용제외공종인 경우
            else if (string.Concat(bid.Element("C6").Value) == "5")
                item = "제요율적용제외";
            //해당 공종이 제요율적용제외공종인 경우
            else if (string.Concat(bid.Element("C6").Value) == "6")
                item = "고정금액";
            //해당 공종이 음의 가격 공종인 경우
            else if (string.Concat(bid.Element("C6").Value) == "7")
                item = "PS내역";
            //해당 공종이 안전관리비인 경우
            else if (string.Concat(bid.Element("C6").Value) == "9")
                item = "안전관리비";
            else
                item = "예외";

            return item;
        }

        static void MatchConstructionNum(string filePath)    //실내역과 xml 데이터 비교를 통해 세부공사별 번호 매칭
        {
            //get workbook
            var workbook = ExcelHandling.GetWorkbook(filePath, ".xlsx");
            //data는 실내역서의 두 번째 sheet인 "내역서"에 위치
            var copySheetIndex = workbook.GetSheetIndex("내역서");
            var sheet = workbook.GetSheetAt(copySheetIndex);
            int check;  //실내역 파일과 세부공사의 데이터가 일치하는 횟수

            //key : 세부공사별 번호 / value : 세부공사별 리스트
            foreach (KeyValuePair<string, List<Data>> dic in Data.Dic)
            {
                check = 0;
                for (int i = 0; i < 5; i++)
                {
                    var row = sheet.GetRow(i + 4);
                    bool sameName = dic.Value[i].Name.Equals(row.GetCell(4).StringCellValue);
                    //품명이 일치하는 경우 데이터 일치
                    if (sameName)
                        check++;
                    //데이터 일치 횟수가 3이 되면 해당 실내역 파일명과 세부공사 번호의 쌍을 딕셔너리에 추가
                    if (check == 3)
                    {
                        Data.MatchedConstNum.Add(filePath, dic.Key);
                        return;
                    }
                }
            }
            //매칭이 되지 않은 경우, 실내역파일과 공내역의 공사가 동일한지 확인해야함.
            Data.IsFileMatch = false;
        }

        static void CopyFile(string filePath)   //실내역파일에서 읽은 데이터로 BID파일에 단가세팅
        {
            var workbook = ExcelHandling.GetWorkbook(filePath, ".xlsx");    //get workbook
            var copySheetIndex = workbook.GetSheetIndex("내역서");          //data는 실내역서의 두 번째 sheet인 "내역서"에 위치
            var sheet = workbook.GetSheetAt(copySheetIndex);

            var constNum = Data.MatchedConstNum[filePath]; //실내역 파일에 해당하는 세부공사 번호 저장
            var lastRowNum = sheet.LastRowNum; //sheet의 마지막 Row Number
            var rowIndex = 4;   //Excel의 row의 인덱스

            //짝이 맞는 Data 객체와 Excel의 row를 찾을 때까지 둘다 행을 늘려감
            foreach (var curObj in Data.Dic[constNum])
            {
                //Data 객체의 코드가 비어있을 경우 다음 객체로 넘어감(빈 경우 또는 공종(입력불가)항목)
                string dcode = curObj.Code;
                if (string.IsNullOrEmpty(dcode))
                    continue;
                var dname = curObj.Name;
                var dunit = curObj.Unit;
                var dquantity = curObj.Quantity;

                //현재 Data 객체와 짝이 맞는 Excel의 Row를 만날 때까지 진행 후, 값의 복사
                while (true)
                {
                    var row = sheet.GetRow(rowIndex);
                    var code = row.GetCell(1).StringCellValue;
                    var name = row.GetCell(4).StringCellValue;
                    var unit = row.GetCell(6).StringCellValue;
                    decimal quantity = 0.0m;
                    //수량이 없는 경우, 다음 행으로 진행 (비어있는 행 또는 공종(입력불가)항목)
                    try
                    {
                        quantity = Convert.ToDecimal(row.GetCell(7).NumericCellValue);
                    }
                    catch
                    {
                        rowIndex++;
                        if (rowIndex == lastRowNum)
                            break;
                        continue;
                    }

                    var sameCode = code.Equals(dcode);
                    var sameName = name.Equals(dname);
                    var sameUnit = unit.Equals(dunit);
                    var sameQuantity = quantity.Equals(dquantity);

                    if ((sameName || sameCode) && (sameUnit || sameQuantity))
                    {
                        curObj.MaterialUnit = Convert.ToDecimal(row.GetCell(8).NumericCellValue); //재료비 단가
                        curObj.LaborUnit = Convert.ToDecimal(row.GetCell(10).NumericCellValue);   //노무비 단가
                        curObj.ExpenseUnit = Convert.ToDecimal(row.GetCell(12).NumericCellValue); //경비 단가
                        rowIndex++;
                        break;
                    }
                    else
                    {
                        rowIndex++;
                        if (rowIndex == lastRowNum)
                            break;
                        continue;
                    }
                }
            }
        }

        static void SetUnitPrice()
        {
            String copiedFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Actual Xlsx";
            DirectoryInfo dir = new DirectoryInfo(copiedFolder);
            FileInfo[] files = dir.GetFiles();

            //실내역으로부터 Data 객체에 단가세팅
            foreach (var file in files)
            {
                MatchConstructionNum(file.FullName);
                if (Data.IsFileMatch)
                    CopyFile(file.FullName);
                else
                    return;
            }

            //복사한 단가 OutputDataFromBID.xml에 세팅
            foreach (var bid in eleBID)
            {
                //단가를 가지는 항목에 단가 복사
                if (bid.Element("C24") != null && string.Concat(bid.Element("C4").Value) == "S")
                {
                    var constNum = string.Concat(bid.Element("C1").Value);      //세부공사 번호
                    var numVal = string.Concat(bid.Element("C2").Value);        //공종 번호
                    var detailVal = string.Concat(bid.Element("C3").Value);     //세부 공종 번호
                    var curObject = Data.Dic[constNum].Find(x => x.WorkNum == numVal && x.DetailWorkNum == detailVal);

                    if (curObject.Item == "일반" || curObject.Item == "제요율적용제외")
                    {
                        bid.Element("C15").Value = curObject.MaterialUnit.ToString();    //재료비 단가
                        bid.Element("C16").Value = curObject.LaborUnit.ToString();       //노무비 단가
                        bid.Element("C17").Value = curObject.ExpenseUnit.ToString();     //경비 단가
                        bid.Element("C18").Value = curObject.UnitPriceSum.ToString();    //합계 단가
                        bid.Element("C19").Value = curObject.Material.ToString();    //재료비
                        bid.Element("C20").Value = curObject.Labor.ToString();       //노무비
                        bid.Element("C21").Value = curObject.Expense.ToString();     //경비
                        bid.Element("C22").Value = curObject.PriceSum.ToString();    //합계
                    }
                }
            }

            //작업 후 xml 파일 저장
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
            using (XmlWriter xw = XmlWriter.Create(sb, xws))
            {
                docBID.WriteTo(xw);
            }
            File.WriteAllText(Path.Combine(Data.folder, "Setting_Xml.xml"), sb.ToString());
        }

        static void GetRate()   //고정금액 및 적용비율 1, 2 저장
        {
            foreach (var bid in eleBID)
            {
                //공사 기간 저장('일' 단위)
                if (bid.Name == "T4")
                {
                    Data.ConstructionTerm = Convert.ToInt64(bid.Element("C28").Value);
                }
                //고정금액 및 적용비율 1, 2 저장
                if (bid.Element("C6") != null)
                {
                    var val = string.Concat(bid.Element("C6").Value);
                    if (val == "6")
                    {
                        var name = string.Concat(bid.Element("C9").Value);  //품명
                        var val1 = string.Concat(bid.Element("C13").Value); //적용비율1
                        var val2 = string.Concat(bid.Element("C14").Value); //적용비율2
                        if (val1 == "0")
                        {
                            long fixedPrice = Convert.ToInt64(bid.Element("C22").Value);    //고정금액
                            Data.Fixed.Add(name, fixedPrice);    //고정금액 딕셔너리에 추가
                        }
                        else
                        {
                            decimal applicationRate1 = Convert.ToDecimal(val1);    //적용비율 1
                            decimal applicationRate2 = Convert.ToDecimal(val2);   //적용비율 2
                            Data.Rate1.Add(name, applicationRate1);  //적용비율1 딕셔너리에 추가
                            Data.Rate2.Add(name, applicationRate2);  //적용비율2 딕셔너리에 추가
                        }
                    }
                }
            }
        }

        public static void GetPrices() //직공비 제외 항목 및 고정금액 계산
        {
            //key : 세부공사별 번호 / value : 세부공사별 리스트
            foreach (KeyValuePair<string, List<Data>> dic in Data.Dic)
            {
                foreach (var item in dic.Value)
                {
                    //해당 공종이 관급자재인 경우
                    if (item.Item.Equals("관급"))
                    {
                        Data.GovernmentMaterial += item.Material;
                        Data.GovernmentLabor += item.Labor;
                        Data.GovernmentExpense += item.Expense;
                    }
                    //해당 공종이 PS인 경우
                    else if (item.Item.Equals("PS"))
                    {
                        Data.PsMaterial += item.Material;
                        Data.PsLabor += item.Labor;
                        Data.PsExpense += item.Expense;
                    }
                    //해당 공종이 제요율적용제외공종인 경우
                    else if (item.Item.Equals("제요율적용제외"))
                    {
                        Data.ExcludingMaterial += item.Material;
                        Data.ExcludingLabor += item.Labor;
                        Data.ExcludingExpense += item.Expense;
                    }
                    //해당 공종이 안전관리비인 경우
                    else if (item.Item.Equals("안전관리비"))
                    {
                        Data.SafetyPrice += item.Expense;
                    }

                    //표준시장단가 품목인지 확인
                    else if (item.Item.Equals("표준시장단가"))
                    {
                        Data.FixedPriceDirectMaterial += item.Material; //재료비 합 계산
                        Data.FixedPriceDirectLabor += item.Labor;    //노무비 합 계산
                        Data.FixedPriceOutputExpense += item.Expense;  //경비 합 계산
                        //직공비에 해당하는 각 객체의 재료비, 노무비, 경비를 직접재료비, 직접노무비, 산출 경비에 더해나감
                        Data.RealDirectMaterial += item.Material;
                        Data.RealDirectLabor += item.Labor;
                        Data.RealOutputExpense += item.Expense;
                        //표준시장 단가 직공비 합계에 더해나감
                        Data.StandardMaterial += item.Material;
                        Data.StandardLabor += item.Labor;
                        Data.StandardExpense += item.Expense;
                    }
                    //음(-)의 단가 품목인지 확인
                    else if (item.Item.Equals("PS내역"))
                    {
                        Data.FixedPriceDirectMaterial += item.Material;
                        Data.FixedPriceDirectLabor += item.Labor;
                        Data.FixedPriceOutputExpense += item.Expense;
                        //직공비에 해당하는 각 객체의 재료비, 노무비, 경비를 직접재료비, 직접노무비, 산출 경비에 더해나감
                        Data.RealDirectMaterial += item.Material;
                        Data.RealDirectLabor += item.Labor;
                        Data.RealOutputExpense += item.Expense;
                    }
                    //직공비 중, 고정금액이 아닌 일반 항목들의 직공비 계산
                    else if (item.Item.Equals("일반"))
                    {
                        Data.RealPriceDirectMaterial += item.Material;
                        Data.RealPriceDirectLabor += item.Labor;
                        Data.RealPriceOutputExpense += item.Expense;
                        //직공비에 해당하는 각 객체의 재료비, 노무비, 경비를 직접재료비, 직접노무비, 산출 경비에 더해나감
                        Data.RealDirectMaterial += item.Material;
                        Data.RealDirectLabor += item.Labor;
                        Data.RealOutputExpense += item.Expense;
                    }
                }
            }
        }

        public static void GetData()    //xml 파일로부터 세부공사별 데이터 저장
        {
            ///공내역 BID -> XML 파일(OutputDataFromBID.xml) 로드
            docBID = XDocument.Load(Path.Combine(Data.folder, "OutputDataFromBID.xml"));
            eleBID = docBID.Root.Elements();

            //세부공사별 번호 Data.ConstructionNums 딕셔너리에 저장
            GetConstructionNum();

            //세부공사별 리스트 생성(Dic -> key : 세부공사별 번호 / value : 세부공사별 리스트)
            AddConstructionList();

            //공내역 xml 파일 읽어들여 데이터 저장
            GetDataFromBID();

            //실내역으로부터 Data 객체에 단가세팅
            SetUnitPrice();

            //고정금액 및 적용비율 저장
            GetRate();

            //직공비 제외항목 및 고정금액 계산
            GetPrices();

            //표준시장단가 합계(조사금액) 저장
            Data.InvestigateStandardMarket = Data.StandardMaterial + Data.StandardLabor + Data.StandardExpense;
        }
    }
}