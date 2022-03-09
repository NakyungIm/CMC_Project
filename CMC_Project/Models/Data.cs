using System;
using System.IO;
using System.Collections.Generic;

namespace SetUnitPriceByExcel
{
    class Data
    {
        public static String folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        // WPF 앱 파일 관리 변수
        public static string XlsText;
        public static IReadOnlyList<FileStream> XlsFiles;
        public static string BidText;
        public static FileStream BidFile;
        public static bool CanCovertFile = false; // 새로운 파일 업로드 시 변환 가능
        public static bool IsConvert = false; // 변환을 했는지 안했는지
        public static bool IsBidFileOk = true; // 정상적인 공내역 파일인지
        public static bool IsFileMatch = true; // 공내역 파일과 실내역 파일의 공사가 일치하는지
        public static double? PersonalRateNum; //
        public static double? BalanceRateNum; // 사정율 출력용 변수


        public static string desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);  //바탕화면 경로
        public static string work_path = Path.Combine(desktop_path,"WORK DIRECTORY");   //작업폴더(WORK DIRECTORY) 경로

        private decimal materialUnit;   //재료비 단가
        private decimal laborUnit;      //노무비 단가
        private decimal expenseUnit;   //경비 단가

        public string Item { get; set; }    //항목 구분(공종(입력불가), 무대(입력불가), 일반, 관급, PS, 제요율적용제외, 안전관리비, PS내역, 표준시장단가)
        public string ConstructionNum { get; set; }     //공사 인덱스
        public string WorkNum { get; set; }             //세부 공사 인덱스
        public string DetailWorkNum { get; set; }       //세부 공종 인덱스
        public string Code { get; set; }        //코드
        public string Name { get; set; }        //품명
        public string Standard { get; set; }    //규격
        public string Unit { get; set; }        //단위
        public decimal Quantity { get; set; }   //수량
        public decimal MaterialUnit //재료비 단가
        {
            get
            {
                //사용자가 단가 정수처리를 원한다면("2") 정수 값으로 return 
                if (UnitPriceTrimming.Equals("2"))
                    return Math.Ceiling(materialUnit);
                return materialUnit;
            }
            set 
            {
                //소수 첫째 자리 아래로 절사한 값을 초기 값으로 세팅
                materialUnit = Math.Truncate(value * 10) / 10;
                //사용자가 단가 정수처리를 원한다면 정수 값으로 세팅 (Reset 함수 사용 시 단가 소수처리 옵션과 상관없이 소수 첫째 자리 아래로 절사) 
                if (UnitPriceTrimming.Equals("2") && ExecuteReset.Equals("0"))
                    materialUnit = Math.Ceiling(value);
            }           
        }
        public decimal LaborUnit //노무비 단가
        {
            get
            { 
                if (UnitPriceTrimming.Equals("2"))
                    return Math.Ceiling(laborUnit);
                return laborUnit;
            }
            set 
            { 
                laborUnit = Math.Truncate(value * 10) / 10;
                if (UnitPriceTrimming.Equals("2") && ExecuteReset.Equals("0"))
                    laborUnit = Math.Ceiling(value);
            }
        }      
        public decimal ExpenseUnit //경비 단가
        {
            get
            {
                if (UnitPriceTrimming.Equals("2"))
                    return Math.Ceiling(expenseUnit);
                return expenseUnit;
            }
            set 
            { 
                expenseUnit = Math.Truncate(value * 10) / 10;
                if (UnitPriceTrimming.Equals("2") && ExecuteReset.Equals("0"))
                    expenseUnit = Math.Ceiling(value);
            }
        }    
        public decimal Material { get { return Math.Truncate(Quantity * MaterialUnit); } }      //재료비 (수량 x 단가)
        public decimal Labor { get { return Math.Truncate(Quantity * LaborUnit); } }            //노무비
        public decimal Expense { get { return Math.Truncate(Quantity * ExpenseUnit); } }        //경비
        public decimal UnitPriceSum { get { return MaterialUnit + LaborUnit + ExpenseUnit; } }  //합계단가
        public decimal PriceSum { get { return Material + Labor + Expense; } }  //합계(세부공종별 금액의 합계)
        public decimal Weight { get; set; }     //가중치
        public decimal PriceScore { get; set; } //세부 점수
        public decimal Score { get { return PriceScore * Weight; } }  //단가 점수(세부 점수 * 가중치)
        
        //원가계산서에 필요한 데이터
        public static long ConstructionTerm { get; set; }       //공사 기간
        public static decimal RealDirectMaterial { get; set; }  //실내역 직접 재료비(일반, - , 표준시장단가)
        public static decimal RealDirectLabor { get; set; }     //실내역 직접 노무비(일반, - , 표준시장단가)
        public static decimal RealOutputExpense { get; set; }   //실내역 산출 경비(일반, - , 표준시장단가)
        public static decimal FixedPriceDirectMaterial { get; set; }    //고정금액 항목 직접 재료비
        public static decimal FixedPriceDirectLabor { get; set; }       //고정금액 항목 직접 노무비
        public static decimal FixedPriceOutputExpense { get; set; }     //고정금액 항목 산출 경비
        public static decimal RealPriceDirectMaterial { get; set; }     //일반항목 직접 재료비
        public static decimal RealPriceDirectLabor { get; set; }        //일반항목 직접 노무비
        public static decimal RealPriceOutputExpense { get; set; }      //일반항목 산출 경비
        public static decimal InvestigateFixedPriceDirectMaterial { get; set; } //고정금액 항목 직접 재료비(조사금액)
        public static decimal InvestigateFixedPriceDirectLabor { get; set; }    //고정금액 항목 직접 노무비(조사금액)
        public static decimal InvestigateFixedPriceOutputExpense { get; set; }  //고정금액 항목 산출 경비(조사금액)
        public static decimal InvestigateStandardMaterial { get; set; }         //표준시장단가 재료비(조사금액)
        public static decimal InvestigateStandardLabor { get; set; }            //표준시장단가 노무비(조사금액)
        public static decimal InvestigateStandardExpense { get; set; }          //표준시장단가 산출경비(조사금액)
        public static decimal PsMaterial { get; set; }  //PS(재료비) 금액(직접 재료비에서 제외)
        public static decimal PsLabor { get; set; }     //PS(노무비) 금액(직접 노무비에서 제외)
        public static decimal PsExpense { get; set; }   //PS(경비) 금액(산출 경비에서 제외)
        public static decimal ExcludingMaterial { get; set; }   //제요율적용제외(재료비) 금액(직접 재료비에서 제외)
        public static decimal ExcludingLabor { get; set; }      //제요율적용제외(노무비) 금액(직접 노무비에서 제외)
        public static decimal ExcludingExpense { get; set; }    //제요율적용제외(경비) 금액(산출 경비에서 제외)
        public static decimal AdjustedExMaterial { get; set; }  //사정율 적용한 제요율적용제외 금액(재료비)
        public static decimal AdjustedExLabor { get; set; }  //사정율 적용한 제요율적용제외 금액(노무비)
        public static decimal AdjustedExExpense { get; set; }  //사정율 적용한 제요율적용제외 금액(경비)
        public static decimal GovernmentMaterial { get; set; }  //관급자재요소(재료비) 금액(직접 재료비에서 제외)
        public static decimal GovernmentLabor { get; set; }     //관급자재요소(노무비) 금액(직접 노무비에서 제외)
        public static decimal GovernmentExpense { get; set; }   //관급자재요소(경비) 금액(산출 경비에서 제외)
        public static decimal SafetyPrice { get; set; }         //안전관리비(산출 경비에서 제외)
        public static decimal StandardMaterial { get; set; }    //표준시장단가 재료비
        public static decimal StandardLabor { get; set; }       //표준시장단가 노무비
        public static decimal StandardExpense { get; set; }     //표준시장단가 산출경비
        public static decimal InvestigateStandardMarket { get; set; }   //표준시장단가 합계(조사내역)
        public static decimal FixedPricePercent { get; set; }           //고정금액 비중

        public static Dictionary<string, List<Data>> Dic = new Dictionary<string, List<Data>>();        //key : 세부공사별 번호 / value : 세부공사별 리스트
        public static Dictionary<string, string> ConstructionNums = new Dictionary<string, string>();   //세부 공사별 번호 저장
        public static Dictionary<string, string> MatchedConstNum = new Dictionary<string, string>();    //실내역과 세부공사별 번호의 매칭 결과

        public static Dictionary<string, long> Fixed = new Dictionary<string, long>();          //고정금액 항목별 금액 저장
        public static Dictionary<string, decimal> Rate1 = new Dictionary<string, decimal>();    //적용비율1 저장
        public static Dictionary<string, decimal> Rate2 = new Dictionary<string, decimal>();    //적용비율2 저장

        public static Dictionary<string, long> RealPrices = new Dictionary<string, long>();     //원가계산서 항목별 금액 저장

        public static Dictionary<string, long> Investigation = new Dictionary<string, long>(); //세부결과_원가계산서 항목별 조사금액 저장
        public static Dictionary<string, long> Bidding = new Dictionary<string, long>();       //세부결과_원가계산서 항목별 입찰금액 저장 
        public static Dictionary<string, long> Correction = new Dictionary<string, long>();     //원가계산서 조사금액 보정 항목 저장

        //사용자의 옵션 및 사정률 데이터
        public static string UnitPriceTrimming { get; set; } = "1";         //단가 소수 처리 (defalut = "1")
        public static string StandardMarketDeduction { get; set; } = "2";   //표준시장단가 99.7% 적용
        public static string ZeroWeightDeduction { get; set; } = "2";     //가중치 0% 공종 50% 적용
        public static string CostAccountDeduction { get; set; } = "2";     //원가계산 제경비 99.7% 적용
        public static string BidPriceRaise { get; set; } = "2";           //투찰금액 천원 절상
        public static string LaborCostLowBound { get; set; } = "2";        //노무비 하한 80%
        public static decimal BalancedRate { get; set; }    //업체 평균 예측율
        public static decimal PersonalRate { get; set; }    //내 예가 사정률
        public static string ExecuteReset { get; set; } = "0";   //Reset 함수 사용시 단가 소수처리 옵션과 별개로 소수 첫째자리 아래로 절사
    }
}