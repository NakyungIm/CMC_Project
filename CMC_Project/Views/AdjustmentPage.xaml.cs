using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SetUnitPriceByExcel;

namespace CMC_Project.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentPage.xaml
    /// </summary>
    public partial class AdjustmentPage : Page
    {
        private static bool isCalculate = false;
        public static bool isConfirm = false;
        public AdjustmentPage()
        {
            InitializeComponent();
            this.averageRating.TextChanged += AverageChangedHandler;
            this.estimateRating.TextChanged += EstimateChangedHandler;
        }

        //sender: 이벤트 발생자, args: 이벤트 인자
        private void AverageChangedHandler(object sender, TextChangedEventArgs args)
        {
            TextBox averageRating = sender as TextBox;
            int selectionStart = averageRating.SelectionStart;
            string result = string.Empty;
            int count = 0;

            foreach (char character in averageRating.Text.ToCharArray())
            {
                if (char.IsDigit(character) || char.IsControl(character) || (character == '.' && count == 0))
                {
                    result += character;
                    if (character == '.')
                    {
                        count += 1;
                    }
                }
            }
            averageRating.Text = result;
            averageRating.SelectionStart = selectionStart <= averageRating.Text.Length ? selectionStart : averageRating.Text.Length;
            //Debug.Print(result);
        }

        //sender: 이벤트 발생자, args: 이벤트 인자
        private void EstimateChangedHandler(object sender, TextChangedEventArgs args)
        {
            TextBox estimateRating = sender as TextBox;
            int selectionStart = estimateRating.SelectionStart;
            string result = string.Empty;
            int count = 0;

            foreach (char character in estimateRating.Text.ToCharArray())
            {
                if (char.IsDigit(character) || char.IsControl(character) || (character == '.' && count == 0))
                {
                    result += character;
                    if (character == '.')
                    {
                        count += 1;
                    }
                }
            }
            estimateRating.Text = result;
            estimateRating.SelectionStart = selectionStart <= estimateRating.Text.Length ? selectionStart : estimateRating.Text.Length;
        }

        private void UpBtnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void CalBtnClick(object sender, RoutedEventArgs e)
        {
            if (averageRating.Text != string.Empty && estimateRating.Text != string.Empty)
            {
                MessageBox.Show("사정율 적용을 완료하였습니다.");

            }
            else
            {
                MessageBox.Show("사정율을 입력해주세요.");
            }
        }

        // 메세지 창
        static public void DisplayDialog(String dialog, String title)
        {
            MessageBox.Show(dialog, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // ------------------------- BID파일 저장 버튼 ---------------------------------------------------------------------------------------------------------------------------------------- //
        private async void SaveBidBtnClick(object sender, System.EventArgs e)
        {
            // TargetRate가 계산 되어 있을 경우
            if (isConfirm)
            {
                //단가 세팅 완료한 xml 파일을 다시 BID 파일로 변환
                //BidHandling.XmlToBid();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //saveFileDialog.Filter = "BID Files (*.BID)|*.BID|All files (*.*)|*.*";
                saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
                saveFileDialog.RestoreDirectory = true;
                //saveFileDialog.Title = "Save a BID File";
                //saveFileDialog.FileName = BidHandling.filename.Substring(0, 16);


                if (saveFileDialog.ShowDialog() == true)
                {
                    //string file = await saveFileDialog.PickSaveFileAsync();
                    //string bidFolder = await Data.folder.GetFolderAsync("Result Bid");
                    //string bidFolder = Data.folder + "\\Result Bid"; //Result Bid 경로
                    //string finalBidFile = bidFolder.GetFiles(BidHandling.filename.Substring(0, 16) + ".BID");


                    saveFileDialog.OverwritePrompt = true;
                    string text = saveFileDialog.FileName;
                    File.WriteAllText(saveFileDialog.FileName, text);

                    DisplayDialog("저장되었습니다.", "Save");
                }
                else
                {
                    DisplayDialog("취소되었습니다.", "Error");
                }
            }

            // 계산 안되어 있을 경우
            else
            {
                DisplayDialog("입찰점수를 계산해주세요.", "Error");
            }
        }

    }
}
