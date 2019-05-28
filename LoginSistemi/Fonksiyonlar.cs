using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace LoginSistemi
{
    public static class Fonksiyonlar
    {
        public static SqliteAccessInterface sqliteVeritabanim = new SqliteAccessInterface();
        
        public static List<WebSitesi> WebSiteleri = new List<WebSitesi>();
        public static HttpStatusCode SiteCevabi { get; private set; }
        
        //Web sitesi değerlerini veritabanını test etmek için tek tek giren fonksiyon.
        public static void WebSitesiDegerleriniAta()
        {

            string[] YnaServisUrlleri = new string[6];
            YnaServisUrlleri[0] = "http://34istmustestapp:8489/";
            YnaServisUrlleri[1] = "http://34istmustestapp:8087/";
            

            WebSitesi YnaTest = new WebSitesi(0,"ynatest.bimar.com", "ctl00_UserInfoLabel", "KullaniciAdiTextBox", "SifreTextBox", "ErrorInfoLabel"
                , "http://ynatest.bimar.com", true, false, YnaServisUrlleri, "yna");

            WebSiteleri.Add(YnaTest);

            string[] ArlesServisUrlleri = new string[6];
            ArlesServisUrlleri[0] = "http://srv1arlesservicetest.bimar.com/";
            ArlesServisUrlleri[1] = "http://34istmustestapp:9050/AmbarService.svc";

            

            WebSitesi ArlesTest = new WebSitesi(1,"arlestest.bimar.com", "UserInfoLabel", "KullaniciAdTextBox", "PwdTextBox", "ErrorLabel",
               "http://arlestest.bimar.com", true, false, ArlesServisUrlleri, "arles");

            WebSiteleri.Add(ArlesTest);

            string[] ArkasAirServisUrlleri = new string[6];

            
            WebSitesi ArkasAirTest = new WebSitesi(2,"arkasairtest.bimar.com", "header", "username", "password", "validation-summary-errors"
                , "http://arkasairtest.bimar.com", false, true, ArkasAirServisUrlleri, "arkasair");

            WebSiteleri.Add(ArkasAirTest);

            string[] ClaimHandlingServisUrlleri = new string[6];
            

            WebSitesi ClaimHandlingTest = new WebSitesi(3,"claimhandlingtest.bimar.com", "LanguageSelect", "UserName", "Password", "field-validation-error",
                "http://claimhandlingtest.bimar.com", false, false, ClaimHandlingServisUrlleri, "claimhandling");

            WebSiteleri.Add(ClaimHandlingTest);

        }

        //Gelen argüman ile test etmeye yarayan fonksiyon.
        public static void argumanIleTestEt(string parametreAdi)
        {

            try
            {
                ChromeDriverSingleton.parametreIleMiGeldi = true;
                
                sqliteVeritabanim.parametreAdinaGoreDegerleriAl(parametreAdi);
                
                for (int i = 0; i < WebSiteleri.Count; i++)
                {

                    LoginIslemiYap(WebSiteleri.ElementAt(i)._siteAdi);

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , WebSiteleri.ElementAt(i)._siteAdi + " basarili bir sekilde test edildi.");

                    //ChromeDriverSingleton.yesilYaz(WebSiteleri.ElementAt(i)._siteAdi + " basarili bir sekilde test edildi.");
                    
                }
                //, "Olgac.OCEK@bimar.com.tr", "Cem.INCE@bimar.com.tr", "idil.akcali@bimar.com.tr" , "_BimarDevreyeAlimEkibiMailGrubu@arkas.com", "soner.turk@bimar.com.tr"
                Fonksiyonlar.EpostaHazırla("noreply@bimar.com.tr", new string[] { "_BimarDevreyeAlimEkibiMailGrubu@arkas.com", "soner.turk@bimar.com.tr", "mustafa.denizalti@bimar.com.tr" }, "Login Kontrol Sonuçları", "Login işlemi sonucu hata mesajları:", ChromeDriverSingleton.epostaMesaji, "");
                

            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, "Batch dosya Parametresi ile web siteleri denenirken bir problem oluştu: " + ex.ToString());

                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Batch dosya Parametresi ile web siteleri denenirken bir problem oluştu: " + ex.ToString());
            }


        }

        //Web sitesi girişi için giriş ekranına giren fonksiyon.
        public static void GirisEkrani()
        {
            
            //Fonksiyon parametleri fonksiyonun içine gömüldü..
            string Url = "", cikisDegeri = "";
            
            while (true)
            {

                //Chrome driver test aracını küçülten fonksiyon..
                //ds.getDriverInstance().Manage().Window.Minimize();

                
                do
                {
                    Console.WriteLine("Lütfen giris yapmak istediğiniz siteyi giriniz:");
                    Url = Console.ReadLine();

                    if (String.IsNullOrEmpty(Url))
                    {
                        Console.WriteLine("Url boş geçilemez..");
                    }

                } while (String.IsNullOrEmpty(Url));
                
                LoginIslemiYap(Url);
                
                do
                {

                    Console.WriteLine("Cikis yapmak icin 0'a basın, devam etmek için herhangi bir olaya basın..");
                    cikisDegeri = Console.ReadLine();

                    if (String.IsNullOrEmpty(cikisDegeri))
                    {
                        Console.WriteLine("Lütfen bir karakter giriniz..");
                    }

                } while (String.IsNullOrEmpty(cikisDegeri));

                
                if (cikisDegeri.Equals("0"))
                {
                    
                    Menu.MenuEkrani();
                    break;
                }

                
            }
        }

        //İlgili web sitesini WebSiteleri arraylistinden getiren fonksiyon.
        public static WebSitesi WebSitesiniGetir(string url)
        {
            //Liste boş değilse..
            if (WebSiteleri.Any())
            {
                foreach (WebSitesi item in WebSiteleri)
                {
                    if (item._url.Equals(url) || item._siteAdi.Equals(url))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        //İlgili url'in status kodunu alan fonksiyon.
        public static HttpStatusCode UrlStatusCodeAl(string url)
        {
            try
            {
                var request = HttpWebRequest.Create(url);
                request.Method = "GET";

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        ChromeDriverSingleton.result = response.StatusCode;
                        response.Close();
                    }
                }
            }
            catch (WebException ex)
            {
                
                if (ChromeDriverSingleton.exceptionCountServisUrlleri < 3)
                {
                    ChromeDriverSingleton.kirmiziYaz("Exception. Bir sonraki deneme için 30 saniye bekliyor..");
                    ChromeDriverSingleton.yesilYaz(url+"  Http status exceptiona düştü. Deneme sayisi: " + (ChromeDriverSingleton.exceptionCountServisUrlleri+1));
                    System.Threading.Thread.Sleep(30000);

                   

                    ChromeDriverSingleton.exceptionCountServisUrlleri++;

                    ChromeDriverSingleton.result = UrlStatusCodeAl(url);
                }
                else
                {

                    ChromeDriverSingleton.kirmiziYaz("Web servisi 3 denemenin ardından cevap vermedi.. : " + ChromeDriverSingleton.exceptionCountServisUrlleri);
                    ChromeDriverSingleton.exceptionCountServisUrlleriSifirla();



                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, url + " url degerine sahip site su hata mesajını vermiştir: " + ex.ToString());

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, url + " url degerine sahip site su hata mesajını vermiştir: " + ex.ToString());
                }
                
            }
            
            return ChromeDriverSingleton.result;
        }

        //Bir web sitesine kullanıcı girişi yapmaya yarayan fonksiyon.
        public static bool KullaniciGirisiYap(IWebDriver driver, string KullaniciAdi, string Sifre, WebSitesi ilgiliWebSitesi, string[] epostaMesaji)
        {

            //Flash animasyonu olan sitelerde bekleme yapmazsak kullanıcı adı ve sifre bolumlerini bulamamaktadır..
            System.Threading.Thread.Sleep(1000);
           
            //string[] errorEtiketim = null;

            try
            {

               

                if (ilgiliWebSitesi._girisEkraniElementiNameMi)
                {
                    driver.FindElement(By.Name(ilgiliWebSitesi._kullaniciAdiEtiketi)).Clear();
                    driver.FindElement(By.Name(ilgiliWebSitesi._kullaniciAdiEtiketi)).SendKeys(KullaniciAdi);
                    driver.FindElement(By.Name(ilgiliWebSitesi._sifreEtiketi)).Clear();
                    driver.FindElement(By.Name(ilgiliWebSitesi._sifreEtiketi)).SendKeys(Sifre + Keys.Enter);
                }
                else
                {
                    driver.FindElement(By.Id(ilgiliWebSitesi._kullaniciAdiEtiketi)).Clear();
                    driver.FindElement(By.Id(ilgiliWebSitesi._kullaniciAdiEtiketi)).SendKeys(KullaniciAdi);
                    driver.FindElement(By.Id(ilgiliWebSitesi._sifreEtiketi)).Clear();
                    driver.FindElement(By.Id(ilgiliWebSitesi._sifreEtiketi)).SendKeys(Sifre + Keys.Enter);
                }


                SiteCevabi = UrlStatusCodeAl(Convert.ToString(driver.Url));

                ChromeDriverSingleton.exceptionCountServisUrlleriSifirla();

                if (!(Convert.ToString(SiteCevabi).Equals("OK")))
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath ,ilgiliWebSitesi._siteAdi + " sunucusuna ait server  response statu code olarak hata mesaji vermistir..");

                    ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sunucusuna ait server hata mesaji vermistir..");

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath , ilgiliWebSitesi._siteAdi + " sunucusuna ait server response statu code olarak hata mesaji vermistir ve sunucunun döndürdüğü cevap: " + SiteCevabi + " hata kodu: 101");
                    
                    epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sunucusuna ait server  response statu code olarak HATA mesaji vermistir ve sunucunun döndürdüğü cevap: " + SiteCevabi + "</font>";

                    ChromeDriverSingleton.ePostaSayisi++;

                    ChromeDriverSingleton.siteCevabiOlumluMu = false;

                    return false;
                }

                
            }
            catch (Exception ex)
            {

                if (InternetBaglantisiVarMi())
                {

                    if (ex is TimeoutException)
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait server giriş işlemi yapılırken "+ ChromeDriverSingleton.timeoutDegeri+ " saniye içerisinde kullanıcı adı ve sifre etiketini yükleyemedi..  Hata:" + ex.ToString());

                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait server giriş işlemi yapılırken" + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde kullanıcı adı ve sifre etiketini yükleyemedi.. Hata:" + ex.ToString());

                        ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sunucusuna ait server giriş işlemi ypaılırken " + ChromeDriverSingleton.timeoutDegeri + " saniye kullanıcı adı ve sifre etiketini yükleyemedi..");

                        epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sunucusuna ait server giriş işlemi yapılırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde Etiketleri yükleyemedi..</font>";

                        ChromeDriverSingleton.ePostaSayisi++;

                        ChromeDriverSingleton.siteCevabiOlumluMu = false;

                        return false;
                    }

                    if (ex is WebDriverTimeoutException)
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , ilgiliWebSitesi._siteAdi + " sunucusuna ait server giriş işlemi yapılırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi.. Hata:" + ex.ToString());

                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait server giriş işlemi yapılırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi hata kodu: 102 Hata:" + ex.ToString());
                        
                        ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sunucusuna ait server " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi..");

                        epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sunucusuna ait server " + ChromeDriverSingleton.timeoutDegeri + " saniye icerisinde CEVAP VERMEDİ..</font>";

                        ChromeDriverSingleton.ePostaSayisi++;

                        ChromeDriverSingleton.siteCevabiOlumluMu = false;

                        return false;
                    }

                    else { 
                        if (ChromeDriverSingleton.exceptionCountWebSiteleri < 3)
                        {

                            ChromeDriverSingleton.yesilYaz(ilgiliWebSitesi._url + " sitesi exceptiona düştü,giriş deneme sayisi: "+ ChromeDriverSingleton.exceptionCountWebSiteleri);

                            ChromeDriverSingleton.exceptionCountWebSiteleri++;
                            ChromeDriverSingleton.kirmiziYaz("Exception. Bir sonraki deneme için 30 saniye bekliyor..");
                            System.Threading.Thread.Sleep(30000);

                            //ChromeDriverSingleton.driver.Quit();
                            //ChromeDriverSingleton.driver = new ChromeDriver();

                            
                            ChromeDriverSingleton.yeniKopyaYarat();



                            ChromeDriverSingleton.getDriverInstance().Url = ilgiliWebSitesi._url;

                            ChromeDriverSingleton.siteCevabiOlumluMu = KullaniciGirisiYap(ChromeDriverSingleton.driver, KullaniciAdi, Sifre, ilgiliWebSitesi, epostaMesaji);

                            if (ChromeDriverSingleton.siteCevabiOlumluMu)
                            {
                                return true;
                            }else
                            {
                                return false;
                            }
                            
                        }else
                        {
                            ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait Server'a ulasilamadi.." + "Deneme sayisi: " + ChromeDriverSingleton.exceptionCountWebSiteleri + " Hata:" + ex.ToString());

                            ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait Server'a ulasilamadi hata kodu: 104 " + "Deneme sayisi: " + ChromeDriverSingleton.exceptionCountWebSiteleri + " Hata:" + ex.ToString());

                            ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sunucusuna ait Server'a ulasilamadi.." + " Deneme sayisi: " + ChromeDriverSingleton.exceptionCountWebSiteleri);

                            epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sunucusuna ait SERVER'A ULAŞILAMADI..</font>";

                            ChromeDriverSingleton.ePostaSayisi++;

                            ChromeDriverSingleton.siteCevabiOlumluMu = false;

                            return false;
                        }
                        
                    }


                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, ilgiliWebSitesi._siteAdi + " sitesine girerken internet baglantısı bulunamadi, Lutfen baglantınızı kontrol edin.. Hata: " + ex.ToString());

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sitesine girerken internet baglantısı bulunamadi hata kodu: 105 Hata:" + ex.ToString());

                    ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sitesine girerken internet baglantısı bulunamadi, Lutfen baglantınızı kontrol edin..");

                    epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sitesine girerken INTERNET BAGLANTISI BULUNAMADI, Lutfen baglantınızı kontrol edin..</font>";

                    ChromeDriverSingleton.ePostaSayisi++;

                    ChromeDriverSingleton.siteCevabiOlumluMu = false;

                    return false;
                }

            }
                System.Threading.Thread.Sleep(5000);
                try
                {
                    
                    driver.FindElement(By.Id(ilgiliWebSitesi._loginOlduktanSonraBakilacakElement));

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , ilgiliWebSitesi._siteAdi + " sitesine giris basarili bir sekilde yapildi..");

                    //ChromeDriverSingleton.yesilYaz(ilgiliWebSitesi._siteAdi + " sitesine giris basarili bir sekilde yapildi..");

                    //Web sitesinin başarılı olduğu mail'e eklenmek istenirse bu yorum satırları silinebilir.
                    // epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='green'>" + ilgiliWebSitesi._siteAdi + " sitesine giris basarili bir sekilde yapildi..</font>";

                    // ChromeDriverSingleton.ePostaSayisi++;

                    ChromeDriverSingleton.siteCevabiOlumluMu = true;
                    return true;
                }
                    
                catch (Exception ex)
                {
                    if (ex is TimeoutException)
                    {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait server login olduktan sonra anasayfa elementi aranırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi..  Hata:" + ex.ToString());

                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait server login olduktan sonra anasayfa elementi aranırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi.. Hata:" + ex.ToString());

                        ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sunucusuna ait server login olduktan sonra anasayfa elementi aranırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi..");

                        epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sunucusuna ait server login olduktan sonra anasayfa elementi aranırken " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi...</font>";

                        ChromeDriverSingleton.ePostaSayisi++;

                        ChromeDriverSingleton.siteCevabiOlumluMu = false;

                        return false;
                    }

                    if (ex is WebDriverTimeoutException)
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , ilgiliWebSitesi._siteAdi + " sunucusuna ait server " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi..  Hata:" + ex.ToString());

                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sunucusuna ait server " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi hata kodu: 106 Hata:" + ex.ToString());

                        ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sunucusuna ait server " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde cevap vermedi..");

                        epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sunucusuna ait server " + ChromeDriverSingleton.timeoutDegeri + " saniye içerisinde CEVAP VERMEDİ..</font>";

                        ChromeDriverSingleton.ePostaSayisi++;

                        ChromeDriverSingleton.siteCevabiOlumluMu = false;

                        return false;
                    }

                    if (ChromeDriverSingleton.exceptionLoginSonrasiKontrolCount < 3)
                    {

                        ChromeDriverSingleton.yesilYaz(ilgiliWebSitesi._url + " sitesi exceptiona düştü,giriş deneme sayisi: " + ChromeDriverSingleton.exceptionLoginSonrasiKontrolCount);

                        ChromeDriverSingleton.exceptionLoginSonrasiKontrolCount++;
                        ChromeDriverSingleton.kirmiziYaz("Exception. Bir sonraki deneme için 30 saniye bekliyor..");
                        System.Threading.Thread.Sleep(30000);
                        ChromeDriverSingleton.driver.Quit();

                        ChromeDriverSingleton.driver = new ChromeDriver();

                        ChromeDriverSingleton.yeniKopyaYarat();

                        ChromeDriverSingleton.getDriverInstance().Url = ilgiliWebSitesi._url;

                        ChromeDriverSingleton.siteCevabiOlumluMu = KullaniciGirisiYap(ChromeDriverSingleton.driver, KullaniciAdi, Sifre, ilgiliWebSitesi, epostaMesaji);

                        if (ChromeDriverSingleton.siteCevabiOlumluMu)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, ilgiliWebSitesi._siteAdi + " sitesine ait server hata mesaji vermistir.. Hata:" + ex.ToString());

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._siteAdi + " sitesine ait server hata mesaji vermistir hata kodu: 107 Hata:" + ex.ToString());

                        ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sitesine ait server hata mesaji vermistir..");

                        epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sitesine ait server HATA mesaji vermistir..</font>";

                        ChromeDriverSingleton.ePostaSayisi++;

                        ChromeDriverSingleton.siteCevabiOlumluMu = false;

                        return false;
                    }
                    
                }
                
            

        }
        
        //İnternet bağlantısının olup olmadığını kontrol eden fonksiyon.
        public static bool InternetBaglantisiVarMi()
        {
            /*
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://www.bing.com/"))
                    {
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Program internet baglantisini kontrol ederken hata mesaji vermistir.. Hata:" + ex.ToString());

                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, "Program internet baglantisini kontrol ederken hata mesaji vermistir.. Hata:" + ex.ToString());

                return false;
            }
            */

            if (ChromeDriverSingleton.InternetAvailability.IsInternetAvailable())
            {
                return true;
            }else
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Program internet baglantisini kontrol ederken hata mesaji vermistir..");

                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, "Program internet baglantisini kontrol ederken hata mesaji vermistir..");

                return false;
            }
        }

        

        //Verilen site adına login işlemi yapan fonksiyon.
        public static void LoginIslemiYap(string SiteAdi)
        {

            string KullaniciAdi = "ygtest", Sifre = "test2015";

            
            ChromeDriverSingleton.yeniKopyaYarat();

            //Sitenin yüklenme süresini 60 sn olarak ayarlar..
            ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ChromeDriverSingleton.timeoutDegeri);
            //Console.WriteLine("  ------" + ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad);

            //Elementin yüklenme süresini 15 saniye olarak ayarlar..
            ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            //Cookieleri silerek tekrardan login ekranina gelinmesi işlemini yapan fonksiyonlar..
            ChromeDriverSingleton.getDriverInstance().Manage().Cookies.DeleteAllCookies();

            string[] epostaMesaji = ChromeDriverSingleton.epostaMesaji;
            
            if (!ChromeDriverSingleton.parametreIleMiGeldi)
            {
                ChromeDriverSingleton.EpostalariSifirla();
                epostaMesaji = ChromeDriverSingleton.epostaMesaji;
            }

            //Tekrar tekrar chromeDriver yaratılmamaktadır..
            IWebDriver driver = ChromeDriverSingleton.getDriverInstance();
            
            bool webSitesiCevapVerdiMi = false;
            
            //Chrome driver ekranının minimum olması sağlandı..
            //driver.Manage().Window.Minimize();

            WebSitesi ilgiliWebSitesi = WebSitesiniGetir(SiteAdi);

            System.Threading.Thread.Sleep(1000);

            if (ilgiliWebSitesi != null)
            {

                if (SiteAdi.Equals(ilgiliWebSitesi._siteAdi))
                {
                    webSitesiCevapVerdiMi = SiteCevapVerdiMi(driver, ilgiliWebSitesi._url, KullaniciAdi, epostaMesaji);
                }
                else
                {
                    webSitesiCevapVerdiMi = SiteCevapVerdiMi(driver, ilgiliWebSitesi._url, KullaniciAdi, epostaMesaji);
                }

                driver = ChromeDriverSingleton.getDriverInstance();

                ChromeDriverSingleton.exceptionSiteCevapVerdiMiCount = 0;

                ChromeDriverSingleton.siteCevabiOlumluMu = false;

                
                if (webSitesiCevapVerdiMi)
                {
                    KullaniciGirisiYap(driver, KullaniciAdi, Sifre,  ilgiliWebSitesi , epostaMesaji);
                }
                else
                {
                    ChromeDriverSingleton.siteCevabiOlumluMu = false;
                }
                
                ChromeDriverSingleton.exceptionCountWebSiteleriSifirla();
                ChromeDriverSingleton.exceptionLoginSonrasiCountSifirla();

                servisUrlleriniKontrolEt(ilgiliWebSitesi, epostaMesaji);

                if (!ChromeDriverSingleton.parametreIleMiGeldi)
                {
                    EpostaGonder(ChromeDriverSingleton.siteCevabiOlumluMu, KullaniciAdi, SiteAdi, epostaMesaji);

                    ChromeDriverSingleton.siteCevabiOlumluMu = false;
                }
               
                webSitesiCevapVerdiMi = false;

            }

            else
            {

                ChromeDriverSingleton.griYaz("İlgili web sitesi veritabaninda bulunamadi..");
            }

        }


        //Sitenin cevap verip vermediğini sorgulayan fonksiyon.
        public static bool SiteCevapVerdiMi(IWebDriver driver, string SiteAdi, string KullaniciAdi, string[] epostaMesaji)
        {
            try
            {
                driver.Url = SiteAdi;

                ChromeDriverSingleton.siteCevabiOlumluMu = true;

                return true;
            }
            catch (Exception ex)
            {
                if (ex is WebDriverTimeoutException)
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , SiteAdi + " web sitesi cevap vermemektedir.. Hata: " + ex.ToString());

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, SiteAdi + " web sitesi cevap vermemektedir hata kodu: 108 Hata:" + ex.ToString());

                    ChromeDriverSingleton.kirmiziYaz(SiteAdi + " web sitesi cevap vermemektedir..");

                    epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>"+SiteAdi + " web sitesi CEVAP VERMEMEKTEDİR..</font>";

                    ChromeDriverSingleton.ePostaSayisi++;

                    ChromeDriverSingleton.siteCevabiOlumluMu = false;

                    return false;
                }

                if (ChromeDriverSingleton.exceptionSiteCevapVerdiMiCount < 3)
                {

                    ChromeDriverSingleton.yesilYaz(SiteAdi + " sitesi driver'a url yüklenirken exceptiona düştü, giriş deneme sayisi: " + ChromeDriverSingleton.exceptionSiteCevapVerdiMiCount);

                    ChromeDriverSingleton.exceptionSiteCevapVerdiMiCount++;

                    ChromeDriverSingleton.kirmiziYaz("Exception. Bir sonraki deneme için 30 saniye bekliyor..");

                    System.Threading.Thread.Sleep(30000);

                    ChromeDriverSingleton.driver.Quit();
                    ChromeDriverSingleton.driver = new ChromeDriver();

                    ChromeDriverSingleton.yeniKopyaYarat();
                    
                    ChromeDriverSingleton.siteCevabiOlumluMu = SiteCevapVerdiMi(ChromeDriverSingleton.driver, SiteAdi, KullaniciAdi, epostaMesaji);

                    if (ChromeDriverSingleton.siteCevabiOlumluMu)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, SiteAdi + " web sitesine giris yapilamamaktadir.. Hata:" + ex.ToString());

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, SiteAdi + " web sitesine giris yapilamamaktadir hata kodu: 109 .. Hata: " + ex.ToString());

                    ChromeDriverSingleton.kirmiziYaz(SiteAdi + " web sitesine giris yapilamamaktadir..");

                    epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + SiteAdi + " web sitesine GIRIS YAPILAMAMAKTADIR..</font>";

                    ChromeDriverSingleton.ePostaSayisi++;

                    ChromeDriverSingleton.siteCevabiOlumluMu = false;

                    return false;
                }
                
            }
        }

        //Eposta göndermesi yapan fonksiyon.
        public static void EpostaGonder(bool GirisYaptiMi, string KullaniciAdi, string Url, string[] Mesaj)
        {
            if (GirisYaptiMi)
            {
                // , "Olgac.OCEK@bimar.com.tr", "Cem.INCE@bimar.com.tr", "idil.akcali@bimar.com.tr", "_BimarDevreyeAlimEkibiMailGrubu@arkas.com", "soner.turk@bimar.com.tr"
                EpostaHazırla("noreply@bimar.com.tr", new string[] { "_BimarDevreyeAlimEkibiMailGrubu@arkas.com", "soner.turk@bimar.com.tr", "mustafa.denizalti@bimar.com.tr" }, "Login Kontrol Sonuçları:", Url + " Sitesine, " + KullaniciAdi + " Kullanıcı Adlı kullanıcı giriş yapmıştır ve şu mesajlari almıştır: ", Mesaj, Url);               
            }
            else
            {
                EpostaHazırla("noreply@bimar.com.tr", new string[] { "_BimarDevreyeAlimEkibiMailGrubu@arkas.com", "soner.turk@bimar.com.tr", "mustafa.denizalti@bimar.com.tr" }, "Login Kontrol Sonuçları:", Url + " Sitesine, " + KullaniciAdi + " Kullanıcı Adlı kullanıcı giriş yapmıştır ve şu mesajlari almıştır: ", Mesaj, Url);                
            }
        }

        //Bir sitenin servis urllerini kontrol eden fonksiyon.
        public static void servisUrlleriniKontrolEt(WebSitesi ilgiliWebSitesi, string[] epostaMesaji)
        {

            if (ilgiliWebSitesi._servisUrlleri != null)
            {

                for (int i = 0; i < ilgiliWebSitesi._servisUrlleri.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ilgiliWebSitesi._servisUrlleri[i]))
                    {
                      ChromeDriverSingleton.yesilYaz("-");

                        ChromeDriverSingleton.result = HttpStatusCode.NotFound;
                        SiteCevabi = UrlStatusCodeAl(ilgiliWebSitesi._servisUrlleri[i]);

                        ChromeDriverSingleton.exceptionCountServisUrlleriSifirla();
                        

                        if (!(Convert.ToString(SiteCevabi).Equals("OK")))
                        {                      

                            ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath ,ilgiliWebSitesi._siteAdi + " sitesine ait " + ilgiliWebSitesi._servisUrlleri[i] + " web servisi hata mesaji vermistir ve hata: "+ SiteCevabi);

                            ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.hataDosyasiPath, ilgiliWebSitesi._servisUrlleri[i] + " web servisi hata mesaji vermistir ve hata: " + SiteCevabi + " hata kodu: 110");
 
                            ChromeDriverSingleton.kirmiziYaz(ilgiliWebSitesi._siteAdi + " sitesine ait " + ilgiliWebSitesi._servisUrlleri[i] + " web servisi hata mesaji vermistir ve hata: " + SiteCevabi);
                            
                            epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='red'>" + ilgiliWebSitesi._siteAdi + " sitesine ait " + ilgiliWebSitesi._servisUrlleri[i] + " web servisi HATA mesaji vermistir ve hata: " + SiteCevabi + "</font>";

                            ChromeDriverSingleton.ePostaSayisi++;
                        }
                        else
                        {
                            ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , ilgiliWebSitesi._siteAdi + " sitesine ait " + ilgiliWebSitesi._servisUrlleri[i] + " web servisi basarili calismaktadir..");

                           // ChromeDriverSingleton.yesilYaz(ilgiliWebSitesi._siteAdi + " sitesine ait " + ilgiliWebSitesi._servisUrlleri[i] + " web servisi basarili calismaktadir..");


                            //Web sitesinin başarılı olduğu mail'e eklenmek istenirse bu yorum satırları silinebilir.
                            //  epostaMesaji[ChromeDriverSingleton.ePostaSayisi] = "<font color='green'>" + ilgiliWebSitesi._siteAdi + " sitesine ait " + ilgiliWebSitesi._servisUrlleri[i] + " web servisi basarili calismaktadir..</font>";

                            //  ChromeDriverSingleton.ePostaSayisi++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        //Alınan hata mesajlarına göre e-posta hazırlayıp gönderen fonksiyon.
        public static void EpostaHazırla(string gonderenMail, string[] gonderilecekMail, string konu, string icerik, string[] epostaMesajlari, string Url)
        {

            if (!epostaMesajlari[0].Equals(""))
            {
                string result = "";

                MailMessage mailMessage = new MailMessage();
                SmtpClient emailServer = new SmtpClient("email.arkas.com");


                mailMessage.From = new MailAddress(gonderenMail, "Login Kontrol..", Encoding.Default);
                mailMessage.Priority = MailPriority.High;

                foreach (string mail in gonderilecekMail)
                {
                    mailMessage.To.Add(mail);
                }

                mailMessage.Subject = konu;

                mailMessage.SubjectEncoding = Encoding.UTF8;

                mailMessage.BodyEncoding = Encoding.UTF8;

                mailMessage.IsBodyHtml = true;

                //String builder nesnesini her posta ile tekrar yaratmamak için hep aynı yerden çekmektedir ve e-postadaki boşlukları ve mesajları birleştirmeye yarar..
                StringBuilder sb = ChromeDriverSingleton.getStringBuilderInstance();

                sb.Append("<br>");
                sb.Append(icerik);
                sb.Append("<br>");

                for (int i = 0; i < epostaMesajlari.Length; i++)
                {
                    if (!string.IsNullOrEmpty(epostaMesajlari[i]))
                    {
                        sb.Append(epostaMesajlari[i]);
                        sb.Append("<br>");
                    }
                    else
                    {
                        break;
                    }
                }

                result = sb.ToString();


                mailMessage.Body = result;

                if (!ChromeDriverSingleton.parametreIleMiGeldi)
                {
                    sb.Clear();
                }


                emailServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                try
                {
                    emailServer.Send(mailMessage);

                    foreach (string mail in gonderilecekMail)
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, mail + " adresine e-posta basarili bir sekilde gonderilmistir..");

                      //  ChromeDriverSingleton.yesilYaz(mail + " adresine e-posta basarili bir sekilde gonderilmistir..");
                    }
                }
                catch (Exception ex)
                {

                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Mail gönderilirken bir hata oluştu. Hata: "+ ex.ToString());


                    foreach (string mail in gonderilecekMail)
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, mail + " adresine e-posta gönderilemedi..");

                        ChromeDriverSingleton.kirmiziYaz(mail + " adresine e-posta gönderilemedi..");
                    }


                }
            }
            else
            {
                if (!Url.Equals(""))
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, Url + " web sitesi basarili calismaktadir..");

                   // ChromeDriverSingleton.yesilYaz(Url + " web sitesi basarili calismaktadir..");

                }
            }
        }


    }
}
