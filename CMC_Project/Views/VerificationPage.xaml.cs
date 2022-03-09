using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SetUnitPriceByExcel;

namespace CMC_Project.Views
{
    /// <summary>
    /// VerificationPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VerificationPage : Window
    {

        public VerificationPage()
        {
            this.InitializeComponent();
            //this.CostAccount_19.TextChanged += CostAccount_19_ValueChanged;
            //this.CostAccount_24.TextChanged += CostAccount_24_ValueChanged;
            //this.CostAccount_28.TextChanged += CostAccount_28_ValueChanged;



            CostAccount_1.Text = Data.Investigation["순공사원가"].ToString("#,##0");
            CostAccount_2.Text = Data.Investigation["직접재료비"].ToString("#,##0");
            CostAccount_3.Text = Data.Investigation["직접재료비"].ToString("#,##0");
            CostAccount_4.Text = Data.Investigation["노무비"].ToString("#,##0");
            CostAccount_5.Text = Data.Investigation["직접노무비"].ToString("#,##0");
            CostAccount_6.Text = Data.Investigation["간접노무비"].ToString("#,##0");
            CostAccount_7.Text = Data.Investigation["경비"].ToString("#,##0");
            CostAccount_8.Text = Data.Investigation["산출경비"].ToString("#,##0");
            CostAccount_9.Text = Data.Investigation["산재보험료"].ToString("#,##0");
            CostAccount_10.Text = Data.Investigation["고용보험료"].ToString("#,##0");
            CostAccount_11.Text = Data.Fixed["국민건강보험료"].ToString("#,##0");
            CostAccount_12.Text = Data.Fixed["노인장기요양보험"].ToString("#,##0");
            CostAccount_13.Text = Data.Fixed["국민연금보험료"].ToString("#,##0");
            CostAccount_14.Text = Data.Fixed["퇴직공제부금"].ToString("#,##0");
            CostAccount_15.Text = Data.Fixed["산업안전보건관리비"].ToString("#,##0");
            CostAccount_16.Text = Data.Fixed["안전관리비"].ToString("#,##0");
            CostAccount_17.Text = Data.Fixed["품질관리비"].ToString("#,##0");
            CostAccount_18.Text = Data.Investigation["환경보전비"].ToString("#,##0");
            CostAccount_19.Text = Data.Investigation["공사이행보증서발급수수료"].ToString("#,##0");
            CostAccount_20.Text = Data.Investigation["건설하도급보증수수료"].ToString("#,##0");
            CostAccount_21.Text = Data.Investigation["건설기계대여대금 지급보증서발급금액"].ToString("#,##0");
            CostAccount_22.Text = Data.Investigation["기타경비"].ToString("#,##0");
            CostAccount_23.Text = Data.Investigation["일반관리비"].ToString("#,##0");
            CostAccount_24.Text = Data.Investigation["이윤"].ToString("#,##0");
            CostAccount_25.Text = Data.Investigation["PS"].ToString("#,##0");
            CostAccount_26.Text = Data.Investigation["제요율적용제외공종"].ToString("#,##0");
            CostAccount_27.Text = Data.Investigation["총원가"].ToString("#,##0");
            CostAccount_28.Text = Data.Investigation["공사손해보험료"].ToString("#,##0");
            CostAccount_29.Text = Data.Investigation["소계"].ToString("#,##0");
            CostAccount_30.Text = Data.Investigation["부가가치세"].ToString("#,##0");
            CostAccount_31.Text = "0";
            CostAccount_32.Text = Data.Investigation["도급비계"].ToString("#,##0");
        }


        static public void DisplayDialog(String dialog, String title)
        {
            MessageBox.Show(dialog, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /*
        private void CostAccount_19_ValueChanged(object sender, TextChangedEventArgs args)
        {
            TextBox CostAccount_19 = sender as TextBox;
            Data.Correction["공사이행보증서발급수수료"] = (long.Parse(CostAccount_19.GetLineText(0)));
        }

        private void CostAccount_24_ValueChanged(object sender, TextChangedEventArgs args)
        {
            TextBox CostAccount_24 = sender as TextBox;
            Data.Correction["이윤"] = (long.Parse(CostAccount_24.GetLineText(0)));
        }

        private void CostAccount_28_ValueChanged(object sender, TextChangedEventArgs args)
        {
            TextBox CostAccount_28 = sender as TextBox;
            Data.Correction["공사손해보험료"] = (long.Parse(CostAccount_28.GetLineText(0)));
        }
        */
        private void CorrectionButton_Click(object sender, RoutedEventArgs e)
        {
            FillCostAccount.CalculateInvestigationCosts(Data.Correction);
            //원가계산서_세부결과 조사금액 세팅
            FillCostAccount.FillInvestigationCosts();
            DisplayDialog("보정 완료!", "Success");
        }

    }
}
