using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace SetUnitPriceByExcel
{
    class BidHandling
    {
        //public static StorageFolder folder = ApplicationData.Current.LocalFolder; // 액세스 허용 구역 (User\AppData\Local\Packages\~~\LocalState) : 앱 임시 데이터
        //String folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        String folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string filename;

        public static async Task BidToXml()
        {
            //string nextName = file.DisplayName + ".zip";
            //await file.RenameAsync(nextName, NameCollisionOption.GenerateUniqueName);

            //StorageFolder copiedFolder = await Data.folder.GetFolderAsync("Empty Bid"); // Empty Bid 폴더
            //IReadOnlyList<StorageFile> bidFile = await copiedFolder.GetFilesAsync();
            String copiedFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\EmptyBid";
            string[] bidFile = Directory.GetFiles(copiedFolder, "*.BID");
            string myfile = bidFile[0];
            //filename = bidFile[0].DisplayName;
            filename = Path.GetFileNameWithoutExtension(bidFile[0]);
            File.Move(myfile, Path.ChangeExtension(myfile, ".zip"));
            ZipFile.ExtractToDirectory(Path.Combine(copiedFolder, filename + ".zip"), copiedFolder);
            string[] files = Directory.GetFiles(copiedFolder, "*.BID");
            string text = File.ReadAllText(files[0]); // 텍스트 읽기
            byte[] decodeValue = Convert.FromBase64String(text);  // base64 변환
            text = Encoding.UTF8.GetString(decodeValue);   // UTF-8로 디코딩
            File.WriteAllText(Path.Combine(Data.folder, "OutputDataFromBID.xml"), text, Encoding.UTF8);

            //실내역 데이터 복사 및 단가 세팅 & 직공비 고정금액 비중 계산
            Setting.GetData();
        }

        public static async void XmlToBid()
        {
            string myfile = Path.Combine(Data.work_path, "Result_Xml.xml");
            byte[] bytes = File.ReadAllBytes(myfile);
            string encodeValue = Convert.ToBase64String(bytes);
            File.WriteAllText(Path.Combine(Data.work_path, "XmlToBID.BID"), encodeValue);
            string resultFileName = filename.Substring(0, 16) + ".zip";
            using (ZipArchive zip = ZipFile.Open(Path.Combine(Data.work_path, resultFileName), ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(Path.Combine(Data.work_path, "XmlToBID.BID"), "XmlToBid.BID");
            }
            File.Move(Path.Combine(Data.work_path, resultFileName), Path.ChangeExtension(Path.Combine(Data.work_path, resultFileName), ".BID"));
        }
    }
}