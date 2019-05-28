using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace LoginSistemi
{
    public static class ChromeDriverSingleton
    {

        public static IWebDriver driver = new ChromeDriver();
        static StringBuilder sb = new StringBuilder();
        public static bool parametreIleMiGeldi = false;
        public static string[] epostaMesaji;
        public static int ePostaSayisi = 0;
        public static string alinanDeger = "";
        public static bool parametreIleMiAcildi = false;
        public static string logDosyasiPath = Environment.CurrentDirectory +"\\LogDosyasi\\logDosyasi.txt";
        public static string veritabaniPath = Environment.CurrentDirectory + "\\webSiteleriDb\\webSiteleri.s3db";
        public static string hataDosyasiPath = Environment.CurrentDirectory + "\\hataliWebSiteleri";
        public static HttpStatusCode result = default(HttpStatusCode);
        public static int exceptionCountServisUrlleri = 0;
        public static int exceptionCountWebSiteleri = 0;
        public static int timeoutDegeri=60;
        public static int exceptionLoginSonrasiKontrolCount = 0;
        public static int exceptionSiteCevapVerdiMiCount = 0;
        public static bool siteCevabiOlumluMu = false;

        public class InternetAvailability
        {
            [DllImport("wininet.dll")]
            private extern static bool InternetGetConnectedState(out int description, int reservedValue);

            public static bool IsInternetAvailable()
            {
                int description;
                return InternetGetConnectedState(out description, 0);
            }
        }

        //Chrome driver'ın static oluşturulmuş kopyasını döndüren fonksiyon.
        public static IWebDriver getDriverInstance()
        {
            
            return driver;

        }

        public static void exceptionLoginSonrasiCountSifirla()
        {
            exceptionLoginSonrasiKontrolCount = 0;
        }
        
        public static void exceptionCountWebSiteleriSifirla()
        {
            exceptionCountWebSiteleri = 0;
        }

        public static void exceptionCountServisUrlleriSifirla()
        {
            exceptionCountServisUrlleri = 0;
        }

        //E-posta mesajı oluşturmak için string arrayi yaratır.
        public static void ePostaArrayiOlustur()
        {
            epostaMesaji = new string[500];

            EpostalariSifirla();

        }

        //Oluşturulmuş e-posta array'ini sıfırlamak için kullanılır.
        public static void EpostalariSifirla()
        {
            //Eposta hata mesajlarını sıfırlar..
            for (int i = 0; i < epostaMesaji.Length; i++)
            {
                epostaMesaji[i] = "";
            }
        }


        //Mevcut chrome driver'i sonlandırıp yeni bir kopya yaratmaya yarayan fonksiyon.
        public static IWebDriver yeniKopyaYarat()
        {

            driver.Quit();

            driver = new ChromeDriver();

            return driver;
        }

        //Static olarak oluşturulmuş string builder nesnesi döndüren fonksiyon.
        public static StringBuilder getStringBuilderInstance()
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }

            return sb;
        }


        //İlgili dosyaya ilgili cümleyi yazan fonksiyon.
        public static bool dosyayaCumleYaz(string filePath, string dosyayaYazilacakCumle)
        {
            string dosyaAdi = "";
            CultureInfo turkiye = CultureInfo.GetCultureInfo("tr-TR");

            String tarihSaat = DateTime.Now.ToString(turkiye);
            
            try
            {
                if(filePath == hataDosyasiPath)
                {
                    dosyaAdi = DateTime.Today.ToShortDateString();
                    File.AppendAllText(filePath+ "\\"+dosyaAdi+".txt", "[ " + tarihSaat + " ]  " + dosyayaYazilacakCumle + Environment.NewLine);
                }else
                {
                    File.AppendAllText(filePath, "[ " + tarihSaat + " ]  " + dosyayaYazilacakCumle + Environment.NewLine);
                }
                return true;
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Log dosyasi olusturulurken hata meydana geldi.. Hata:" + ex.ToString());
                Console.WriteLine("Dosya olusturulamadi..");
                return false;
            }
            
        }
    

        //Kaç ay isteniyorsa o kadar geriye gidilerek dosyaları silen fonksiyon.
        public static void eskiAyaAitDosyalariSil(int kacAyOncesi)
        {

            CultureInfo turkiye = CultureInfo.GetCultureInfo("tr-TR");
            string cur_dir = Environment.CurrentDirectory;
            DirectoryInfo d = new DirectoryInfo(cur_dir);
            FileInfo[] Files = d.GetFiles("*.txt");
            DateTime silinecek_tarih = DateTime.Today.AddMonths(-1*kacAyOncesi);
            string ay = silinecek_tarih.Month.ToString();
            string yıl = silinecek_tarih.Year.ToString();
            foreach (FileInfo file in Files)
            {
                if (file.Name.Contains(ay + "." + yıl))
                {
                    yesilYaz(file.Name + " adlı dosya otomatik olarak temizlendi.(Eski aylara ait dosya.)\n");
                    file.Delete();
                }
            }
        }

        //Konsol ekranına kırmızı yazmaya yarayan fonksiyon.
        public static void kirmiziYaz(string metin)
        {
            var original_color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(metin);
            Console.ForegroundColor = original_color;
        }

        //Konsol ekranına yeşil yazmaya yarayan fonksiyon.
        public static void yesilYaz(string metin)
        {
            var original_color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(metin);
            Console.ForegroundColor = original_color;
        }

        //Konsol ekranına gri yazmaya yarayan fonksiyon.
        public static void griYaz(string metin)
        {
            var original_color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(metin);
            Console.ForegroundColor = original_color;
        }






    }
}
