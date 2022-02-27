using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using SetUnitPriceByExcel;
using System.ComponentModel;
using System.Security.AccessControl;
using System.Security.Principal;

namespace CMC_Project.Views
{
    /// <summary>
    /// Interaction logic for ConvertionPage.xaml
    /// </summary>
    public partial class ConvertionPage : Page
    {
        public ConvertionPage()
        {
            InitializeComponent();


            if (Data.XlsFiles != null)
            {
                XlsList.Text = Data.XlsText;
            }
            if (Data.BidFile != null)
            {
                BIDList.Text = Data.BidText;
            }
            if (!Data.CanCovertFile && Data.IsConvert)
            {
                BidOpenFile.IsEnabled = false;
                XlsOpenFile.IsEnabled = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // 메세지 창
        static public void DisplayDialog(String dialog, String title)
        {
            MessageBox.Show(dialog, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Bid File Open
        private async void BIDOpenClick(object sender, RoutedEventArgs e)
        {
            // 파일 탐색기 열기
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "BID Files (*.BID)|*.BID|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == true)
            {
                // 복사 파일 저장 폴더 생성
                String copiedFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\EmptyBid";
                String copiedFolder2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WORK DIRECTORY";

                if (!Directory.Exists(copiedFolder)) // 이미 폴더가 있지 않은 경우
                {
                    // directory permission
                    Directory.CreateDirectory(copiedFolder);
                    DirectoryInfo info = new DirectoryInfo(copiedFolder);
                    info.Attributes &= ~FileAttributes.ReadOnly; // not read only 
                    // access control
                    DirectorySecurity security = info.GetAccessControl();
                    security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    info.SetAccessControl(security);

                    // directory permission
                    Directory.CreateDirectory(copiedFolder2);
                    DirectoryInfo info2 = new DirectoryInfo(copiedFolder2);
                    info2.Attributes &= ~FileAttributes.ReadOnly; // not read only 
                    // access control
                    DirectorySecurity security2 = info.GetAccessControl();
                    security2.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    info2.SetAccessControl(security2);

                    FileStream file;

                    // 파일 복사
                    using (FileStream SourceStream = File.Open(openFileDialog.FileName, FileMode.Open))
                    {
                        using (FileStream DestinationStream = File.Create(copiedFolder + "\\" + System.IO.Path.GetFileName(openFileDialog.FileName)))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                            file = DestinationStream;
                            DisplayDialog(DestinationStream.Name, "확인");
                        }
                    }

                    Data.BidText = System.IO.Path.GetFileName(openFileDialog.FileName);
                    BIDList.Text = Data.BidText;
                    Data.BidFile = file;

                    Data.CanCovertFile = true;
                    Data.IsConvert = false;
                }
                else
                {
                    DisplayDialog("Empty BID 폴더가 이미 존재합니다.", openFileDialog.FileName);
                    Data.CanCovertFile = false;
                    Data.IsConvert = false;
                }

            }
            else
            {
                DisplayDialog("파일을 업로드 해주세요.", "Error");
                Data.XlsFiles = null;
            }
        }

        private async void XlsOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Xls 파일(*.xls, *.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*"; // TODO : 왜 안 먹냐?
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == true)
            {
                String copiedFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Actual Xlsx";
                StringBuilder output = new StringBuilder();

                if (!Directory.Exists(copiedFolder)) // 이미 폴더가 있지 않은 경우
                {

                    // directory permission
                    Directory.CreateDirectory(copiedFolder);
                    DirectoryInfo info = new DirectoryInfo(copiedFolder);
                    info.Attributes &= ~FileAttributes.ReadOnly; // not read only 

                    // access control
                    DirectorySecurity security = info.GetAccessControl();
                    security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    info.SetAccessControl(security);

                    int filenum = openFileDialog.FileNames.Length;
                    List<FileStream> files = new List<FileStream>(new FileStream[filenum]);
                    int count = 0;

                    foreach (string filepath in openFileDialog.FileNames)
                    {
                        String filename = System.IO.Path.GetFileName(filepath);
                        output.Append(filename + "\n");
                        // 파일 복사

                        using (FileStream SourceStream = File.Open(filepath, FileMode.Open))
                        {
                            using (FileStream DestinationStream = File.Create(copiedFolder + "\\" + filename))
                            {
                                await SourceStream.CopyToAsync(DestinationStream);
                                files[count] = DestinationStream;
                            }
                        }
                        count++;
                    }

                    Data.XlsFiles = files;
                    Data.XlsText = output.ToString();
                    XlsList.Text = Data.XlsText;

                    Data.CanCovertFile = true;
                    Data.IsConvert = false;
                    count = 0;
                }
                else
                {
                    DisplayDialog("Actual Xlsx 폴더가 이미 존재합니다.", "Error");
                    Data.CanCovertFile = false;
                    Data.IsConvert = false;
                }
            }
            else
            {
                DisplayDialog("파일을 업로드 해주세요.", "Error");
                Data.XlsFiles = null;
            }
        }



        private async void ConvertButtonClick(object sender, RoutedEventArgs e)
        {

            if (Data.XlsFiles == null || Data.BidFile == null)
            {
                DisplayDialog("파일을 업로드 해주세요!.", "Upload");
            }
            else if (!Data.CanCovertFile)
            {

                DisplayDialog("이미 변환을 완료한 파일입니다. \n새로운 파일 업로드 혹은 저장 버튼을 눌러주세요.", "Error");
            }
            else
            {
                BidOpenFile.IsEnabled = false;
                XlsOpenFile.IsEnabled = false;
                try
                {
                    //공내역 bid 파일 -> 공내역 xml 파일
                    await BidHandling.BidToXml();
                    //실내역 데이터 복사 및 단가 세팅 & 직공비 고정금액 비중 계산
                    //Setting.GetData(); -> 비동기 문제로 BidHandling.BidToXml()로 이동
                }
                catch
                {
                    if (!Data.IsFileMatch)
                    {
                        DisplayDialog("공내역 파일과 실내역 파일의 공사가 일치하는지 확인해주세요.", "Error");
                        return;
                    }
                    DisplayDialog("정상적인 파일이 아닙니다. 파일을 확인해주세요.", "Error");
                    return;
                }
                if (!Data.IsBidFileOk)
                {
                    DisplayDialog("정상적인 공내역 파일이 아닙니다. 파일을 확인해주세요.", "Error");
                    return;
                }
                else if (!Data.IsFileMatch)
                {
                    DisplayDialog("공내역 파일과 실내역 파일의 공사가 일치하는지 확인해주세요.", "Error");
                    return;
                }
                else
                {
                    //원가계산서상 없는 항목들 예외 처리(0 대입)
                    FillCostAccount.CheckKeyNotFound();

                    //원가계산서 항목별 조사금액 계산(보정 전)
                    FillCostAccount.CalculateInvestigationCosts(Data.Correction);

                    //ViewCostAccount();

                    Data.CanCovertFile = false;
                    Data.IsConvert = true;
                    AdjustmentPage.isConfirm = true;
                    DisplayDialog("단가 세팅 완료", "Complete");
                    //isConfirm = true;

                }
            }
        }

        /*
        private async void ViewCostAccount()
        {
            CoreApplicationView newCoreView = CoreApplication.CreateNewView();
            ApplicationView newAppView = null;
            int mainViewId = ApplicationView.GetApplicationViewIdForWindow(
                CoreApplication.MainView.CoreWindow);
            await newCoreView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                newAppView = ApplicationView.GetForCurrentView();
                Window.Current.Content = new Frame();
                (Window.Current.Content as Frame).Navigate(typeof(VerificationPage));
                Window.Current.Activate();
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.UseHalf, mainViewId, ViewSizePreference.UseHalf);
        }
        */

        private async void InitButtonClick(object sender, RoutedEventArgs e)
        {
            //앱 데이터 초기화
            //await ApplicationData.Current.ClearAsync();

            // Data 초기화
            Data.ConstructionTerm = 0;
            Data.RealDirectMaterial = 0;
            Data.RealDirectLabor = 0;
            Data.RealOutputExpense = 0;
            Data.FixedPriceDirectMaterial = 0;
            Data.FixedPriceDirectLabor = 0;
            Data.FixedPriceOutputExpense = 0;
            Data.RealPriceDirectMaterial = 0;
            Data.RealPriceDirectLabor = 0;
            Data.RealPriceOutputExpense = 0;
            Data.InvestigateFixedPriceDirectMaterial = 0;
            Data.InvestigateFixedPriceDirectLabor = 0;
            Data.InvestigateFixedPriceOutputExpense = 0;
            Data.InvestigateStandardMaterial = 0;
            Data.InvestigateStandardLabor = 0;
            Data.InvestigateStandardExpense = 0;
            Data.PsMaterial = 0;
            Data.PsLabor = 0;
            Data.PsExpense = 0;
            Data.ExcludingMaterial = 0;
            Data.ExcludingLabor = 0;
            Data.ExcludingExpense = 0;
            Data.AdjustedExMaterial = 0;
            Data.AdjustedExLabor = 0;
            Data.AdjustedExExpense = 0;
            Data.GovernmentMaterial = 0;
            Data.GovernmentLabor = 0;
            Data.GovernmentExpense = 0;
            Data.SafetyPrice = 0;
            Data.StandardMaterial = 0;
            Data.StandardLabor = 0;
            Data.StandardExpense = 0;
            Data.InvestigateStandardMarket = 0;
            Data.FixedPricePercent = 0;

            //자료구조 초기화
            Data.Dic.Clear();
            Data.ConstructionNums.Clear();
            Data.MatchedConstNum.Clear();
            Data.Fixed.Clear();
            Data.Rate1.Clear();
            Data.Rate2.Clear();
            Data.RealPrices.Clear();
            Data.Investigation.Clear();
            Data.Bidding.Clear();
            Data.Correction.Clear();

            //변수 초기화
            Data.XlsFiles = null;
            Data.BidFile = null;
            Data.CanCovertFile = false;
            Data.IsConvert = false;
            Data.IsBidFileOk = true;
            Data.IsFileMatch = true;

            //업로드 버튼 활성화
            BidOpenFile.IsEnabled = true;
            XlsOpenFile.IsEnabled = true;
            //isConfirm = false;

            //텍스트 초기화
            XlsList.Text = "파일을 업로드 해주세요.";
            BIDList.Text = "파일을 업로드 해주세요.";
            DisplayDialog("초기화 되었습니다.", "Init");

            //WPF 변수 초기화
            Data.BalanceRateNum = null;
            Data.PersonalRateNum = null;
            Data.UnitPriceTrimming = "1";
            Data.StandardMarketDeduction = "2";
            Data.ZeroWeightDeduction = "2";
            Data.CostAccountDeduction = "2";
            Data.BidPriceRaise = "2";
            Data.LaborCostLowBound = "2";
            Data.ExecuteReset = "0";
            CalculatePrice.targetRate = 0;
            CalculatePrice.myPercent = 0;
        }

        private async void AdjustBtnClick(object sender, RoutedEventArgs e)
        {
            AdjustmentPage adjustmentPage = new AdjustmentPage();
            NavigationService.Navigate(adjustmentPage);
        }

        
    }
}
