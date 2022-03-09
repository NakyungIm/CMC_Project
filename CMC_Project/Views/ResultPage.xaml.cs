using System.Windows;
using SetUnitPriceByExcel;


namespace CMC_Project.Views
{
    /// <summary>
    /// ResultPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultPage : Window
    {

        public ResultPage()
        {
            InitializeComponent();


            // 조사금액
            InvestigateDirectMaterial.Text = Data.Investigation["직접재료비"].ToString("#,##0"); //직접재료비
            InvestigateDirectLabor.Text = Data.Investigation["직접노무비"].ToString("#,##0"); //직접노무비
            InvestigateOutputExpense.Text = Data.Investigation["산출경비"].ToString("#,##0"); //산출경비
            InvestigateSum.Text = Data.Investigation["직공비"].ToString("#,##0"); // 직공비 소계
            InvestigateContract.Text = Data.Investigation["도급비계"].ToString("#,##0"); // 도급비계
            BeforeExcludedItem.Text = Data.Investigation["제요율적용제외공종"].ToString("#,##0"); // 제요율적용제외
            BeforeStandardItem.Text = Data.InvestigateStandardMarket.ToString("#,##0"); // 표준시장단가
            BeforePSitem.Text = Data.Investigation["PS"].ToString("#,##0"); // PS단가

            // 입찰금액
            RealDirectMaterial.Text = Data.Bidding["직접재료비"].ToString("#,##0"); // 직접재료비
            RealDirectLabor.Text = Data.Bidding["직접노무비"].ToString("#,##0"); // 직접노무비
            RealOutputExpense.Text = Data.Bidding["산출경비"].ToString("#,##0"); // 산출경비
            RealSum.Text = Data.Bidding["직공비"].ToString("#,##0"); // 직공비 소계
            RealContract.Text = Data.Bidding["도급비계"].ToString("#,##0"); // 도급비계
            AfterExcludedItem.Text = Data.Bidding["제요율적용제외공종"].ToString("#,##0"); // 제요율적용제외
            AfterStandardItem.Text = (Data.StandardMaterial + Data.StandardLabor + Data.StandardExpense).ToString("#,##0"); // 표준시장단가
            AfterPSitem.Text = Data.Bidding["PS"].ToString("#,##0"); // PS단가

            // 조사대비율 : 열은 동일
            DMpercent.Text = (double)FillCostAccount.GetRate("직접재료비") + " %";
            DLpercent.Text = (double)FillCostAccount.GetRate("직접노무비") + " %";
            OEpercent.Text = (double)FillCostAccount.GetRate("산출경비") + " %";
            SUMpercent.Text = ((double)Data.Bidding["직공비"] / Data.Investigation["직공비"] * 100).ToString("F2") + " %";
            CONTRACTpercent.Text = (double)FillCostAccount.GetRate("도급비계") + " %";
            EIpercent.Text = (double)FillCostAccount.GetRate("제요율적용제외공종") + " %";
            SIpercent.Text = ((Data.StandardMaterial + Data.StandardLabor + Data.StandardExpense) / Data.InvestigateStandardMarket * 100).ToString("F2") + " %";
            PSpercent.Text = (double)FillCostAccount.GetRate("PS") + " %";
        }

    }
}
