using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSistemi
{
    public static class Menu
    {

        //Menu ekranını açan fonksiyon.
        public static void MenuEkrani()
        {
            string alinanDeger = "";

            do
            {

               //Fonksiyonlar.ds.getDriverInstance().Manage().Window.Minimize();

                Console.WriteLine("-------------MENU-------------");
                Console.WriteLine();
                Console.WriteLine("1. Var olan siteleri ve özelliklerini görüntüle..");
                Console.WriteLine("2. Yeni site ekle..");
                Console.WriteLine("3. Var olan siteyi sil..");
                Console.WriteLine("4. Var olan sitenin bilgilerini güncelle..");
                Console.WriteLine("5. Var olan Siteye giriş işlemi yap..");
                Console.WriteLine("6. Cikis");
                alinanDeger = Console.ReadLine();

                if (string.IsNullOrEmpty(alinanDeger))
                {
                    Console.WriteLine("Lütfen bir deger giriniz..");
                }
                else if (!(alinanDeger.Equals("1") || alinanDeger.Equals("2") || alinanDeger.Equals("3") || alinanDeger.Equals("4") || alinanDeger.Equals("5") || alinanDeger.Equals("6")))
                {
                    Console.WriteLine("Lutfen menude var olan degerlerden birini giriniz..");
                }
                else
                {
                    break;
                }

            } while (true);

            islemEkrani(alinanDeger);
        }

        //Menu ekranından alınan değerin kullanıldığı fonksiyon.
        public static void islemEkrani(string alinanDeger)
        {

            try
            {
                if (alinanDeger.Equals("1"))
                {
                    //Var olan siteleri görüntüler..
                    Console.WriteLine();

                    //Fonksiyonlar.ds.getDriverInstance().Manage().Window.Minimize();

                    //ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);

                    //Web sitesi değerlerini sqlite'dan alan fonksiyon..
                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    varOlanSiteleriGoruntule();

                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    MenuEkrani();
                }
                else if (alinanDeger.Equals("2"))
                {

                    //Fonksiyonlar.ds.getDriverInstance().Manage().Window.Minimize();

                    //ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);

                    //Web sitesi değerlerini sqlite'dan alan fonksiyon..
                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    // Yeni site eklemesi yapar..
                    yeniSiteEklemeEkrani();

                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    MenuEkrani();
                }
                else if (alinanDeger.Equals("3"))
                {
                    //Fonksiyonlar.ds.getDriverInstance().Manage().Window.Minimize();

                    //ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);

                    //Web sitesi değerlerini sqlite'dan alan fonksiyon..
                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    //Var olan siteyi siler..
                    varOlanSiteyiSil();

                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    MenuEkrani();
                }
                else if (alinanDeger.Equals("4"))
                {
                    //Fonksiyonlar.ds.getDriverInstance().Manage().Window.Minimize();

                    //ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);

                    //Web sitesi değerlerini sqlite'dan alan fonksiyon..
                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    //Var olan sitenin bilgilerini günceller..
                    VarOlanSiteninBilgileriniGuncelle();

                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    MenuEkrani();


                }
                else if (alinanDeger.Equals("5"))
                {
                    //Fonksiyonlar.ds.getDriverInstance().Manage().Window.Minimize();

                    ChromeDriverSingleton.getDriverInstance().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);

                    //Web sitesi değerlerini sqlite'dan alan fonksiyon..
                    Fonksiyonlar.sqliteVeritabanim.veritabaniDegerleriniListeyeAta();

                    Fonksiyonlar.GirisEkrani();
                }
                else if (alinanDeger.Equals("6"))
                {
                    ChromeDriverSingleton.getDriverInstance().Quit();
                    return;
                }
                else
                {
                    Console.WriteLine("Hatali giris yaptiniz lutfen tekrar giris yapiniz..");
                    MenuEkrani();
                }
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Menu ekranında hata meydana geldi.. Hata:" + ex.ToString());
                
            }

        }

        //Var olan sitenin bilgilerini guncelleyen fonksiyon.
        public static void VarOlanSiteninBilgileriniGuncelle()
        {
            string alinanId = "", alinanDeger = "", boolDeger = "", alinanDegerinDegeri = "", servisUrl = "", alinanParametreninAdi = "";
            bool booleanDegerim = false;
            int id = 0, sayac = 0, menudenSecilen = 0;


            string[] servisUrlleri = new string[500];


            Console.WriteLine();
            Console.WriteLine("-----VAR OLAN SITENIN BILGILERINI GUNCELLEME EKRANI-----");

            varOlanSiteleriGoruntule();
            Console.WriteLine();

            do
            {
                Console.WriteLine("Lutfen bilgilerini duzenlemek istediginiz sitenin Id'sini giriniz..");
                alinanId = Console.ReadLine();

                if (string.IsNullOrEmpty(alinanId))
                {
                    Console.WriteLine("Id numarasi bos gecilemez..");
                }
                else
                {
                    try
                    {
                        id = Convert.ToInt32(alinanId);

                        if (Fonksiyonlar.sqliteVeritabanim.idDegeriVarMi(id))
                            break;
                        else
                        {
                            Console.WriteLine(id + " idli item veritabaninda bulunmamaktadir..");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Lutfen gecerli bir id numarasi giriniz..");
                    }
                }

            } while (true);



            do
            {
                Console.WriteLine("Lutfen degistirilmek istenen parametrenin numarasini giriniz:");
                Console.WriteLine("1). siteAdi");
                Console.WriteLine("2). loginOlduktanSonraBakilacakElement");
                Console.WriteLine("3). kullaniciAdiEtiketi");
                Console.WriteLine("4). sifreEtiketi");
                Console.WriteLine("5). errorEtiketi");
                Console.WriteLine("6). url");
                Console.WriteLine("7). errorEtiketiIdMi");
                Console.WriteLine("8). girisEkraniElementiNameMi");
                Console.WriteLine("9). servisUrlleri");
                Console.WriteLine("10).parametre");

                alinanDeger = Console.ReadLine();

                if (string.IsNullOrEmpty(alinanDeger))
                {
                    Console.WriteLine("Degistirilmek istenen parametre degeri bos gecilemez..");
                }
                else
                {
                    try
                    {
                        menudenSecilen = Convert.ToInt32(alinanDeger);

                        if (menudenSecilen == 1 || menudenSecilen == 2 || menudenSecilen == 3 || menudenSecilen == 4 || menudenSecilen == 5 || menudenSecilen == 6 || menudenSecilen == 7 || menudenSecilen == 8 || menudenSecilen == 9 || menudenSecilen == 10)
                        {
                            break;

                        }
                        else
                        {
                            Console.WriteLine("Lutfen menudeki rakamlardan bir tanesini seciniz..");
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Lutfen gecerli bir menu numarasi giriniz..");
                    }
                }

            } while (true);


            //Menuden secilen parametrenin adini girmektedir..
            alinanParametreninAdi = parametreIsmiAl(menudenSecilen);


            if (menudenSecilen == 7 || menudenSecilen == 8)
            {

                do
                {
                    Console.WriteLine(alinanParametreninAdi + " degerine eklemek istediginiz bool degeri giriniz.. (Orn: 1:true ya da 0:false):");
                    boolDeger = Console.ReadLine();

                    if (String.IsNullOrEmpty(boolDeger))
                    {
                        Console.WriteLine("bool degeri bos gecilemez..");
                    }
                    else
                    {
                        try
                        {
                            booleanDegerim = Convert.ToBoolean(Convert.ToInt32(boolDeger));
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(alinanDeger + " degeri tanimlanamadi.. 1 ya da 0 giriniz..");
                        }
                    }

                } while (true);


                if (Fonksiyonlar.sqliteVeritabanim.SQLiteVeritabanindakiVeriyiDüzelt(id, alinanParametreninAdi, booleanDegerim))
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath , id + " id numarali + " + alinanParametreninAdi + " degeri basarili bir sekilde guncellenmistir..");

                    ChromeDriverSingleton.yesilYaz("Guncelleme islemi basarili bir sekilde yapilmistir..");
                }

                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " id numarali + " + alinanParametreninAdi + " degeri guncellenememistir..");
                    ChromeDriverSingleton.kirmiziYaz("Guncelleme islemi basarisiz olmustur..");
                }

            }
            else if (menudenSecilen == 9)
            {
                do
                {
                    Console.WriteLine(alinanParametreninAdi + " ait olan verileri giriniz: (Örn: http://34istmustestapp:8489/)");
                    Console.WriteLine("Url girisini bitirmek için 0'a basınız..");
                    servisUrl = Console.ReadLine();


                    if (String.IsNullOrEmpty(servisUrl))
                    {
                        Console.WriteLine("Servis url'i değeri boş geçilemez..");
                    }
                    else
                    {
                        try
                        {

                            if (servisUrl.Equals("0"))
                            {
                                break;
                            }

                            if (sayac == 500)
                            {
                                Console.WriteLine("Url ekleme maximum sayiya ulasmistir..");
                                Console.WriteLine("Su ana kadar eklenen urller kaydedilmistir..");
                            }

                            servisUrlleri[sayac] = servisUrl;
                            sayac++;

                            Console.WriteLine("Eklemek icin url girisine devam edin, girisi bitirmek icin 0'a basınız..");

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Servis url'i degeri tanimlanamadi..");
                        }
                    }

                } while (true);

                sayac = 0;

                if (Fonksiyonlar.sqliteVeritabanim.SQLiteVeritabanindakiVeriyiDüzelt(id, servisUrlleri))
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " id numarali web sitesinin servis Urlleri degeri basarili bir sekilde guncellenmistir..");
                    ChromeDriverSingleton.yesilYaz("Guncelleme islemi basarili bir sekilde yapilmistir..");
                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " id numarali web sitesinin servis Urlleri degeri guncellenememistir..");
                    ChromeDriverSingleton.kirmiziYaz("Guncelleme islemi basarisiz bir sekilde tamamlanmistir..");
                }

            }else
            {

                do
                {
                    Console.WriteLine(alinanDeger + " verisinin degerini giriniz: ");
                    alinanDegerinDegeri = Console.ReadLine();

                    if (String.IsNullOrEmpty(alinanDegerinDegeri))
                    {
                        Console.WriteLine(alinanDeger + " bos gecilemez..");
                    }
                    else
                    {
                        break;
                    }

                } while (true);

                if (Fonksiyonlar.sqliteVeritabanim.SQLiteVeritabanindakiVeriyiDüzelt(id, alinanParametreninAdi, alinanDegerinDegeri))
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " id numarali web sitesinin " + alinanParametreninAdi+ " degeri basarili bir sekilde guncellenmistir..");
                    ChromeDriverSingleton.yesilYaz("Guncelleme islemi basarili bir sekilde yapilmistir..");
                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " id numarali web sitesinin " + alinanParametreninAdi + " degeri guncellenememistir..");
                    ChromeDriverSingleton.kirmiziYaz("Guncelleme islemi basarisiz bir sekilde tamamlanmistir..");
                }

            }



        }

        //Veri güncelleme ekranında seçilen parametrenin adını döndüren fonksiyon.
        public static string parametreIsmiAl(int menudenSecilen)
        {
            switch (menudenSecilen)
            {
                case 1:
                    return "siteAdi";
                case 2:
                    return "loginOlduktanSonraBakilacakElement";
                case 3:
                    return "kullaniciAdiEtiketi";
                case 4:
                    return "sifreEtiketi";
                case 5:
                    return "errorEtiketi";
                case 6:
                    return "url";
                case 7:
                    return "errorEtiketiIdMi";
                case 8:
                    return "girisEkraniElementiNameMi";
                case 9:
                    return "servisUrlleri";
                case 10:
                    return "parametre";
                default:
                    return null;
            }
        }



        //Veritabanındaki var olan siteyi silmeye yarayan fonskyion.
        public static void varOlanSiteyiSil()
        {
            string alinanId = "";
            int id = 0;

            Console.WriteLine();
            Console.WriteLine("-----VAR OLAN SITEYI SILME EKRANI-----");

            varOlanSiteleriGoruntule();
            Console.WriteLine();

            do
            {
                Console.WriteLine("Lütfen silmek istediginiz sitenin Id'sini giriniz..");
                alinanId = Console.ReadLine();

                if (string.IsNullOrEmpty(alinanId))
                {
                    Console.WriteLine("Lutfen gecerli bir id numarasi giriniz..");
                }
                else
                {
                    try
                    {
                        id = Convert.ToInt32(alinanId);

                        if (Fonksiyonlar.sqliteVeritabanim.idDegeriVarMi(id))
                            break;
                        else
                        {
                            Console.WriteLine(id + " idli item veritabaninda bulunmamaktadir..");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Lütfen gecerli bir id numarasi giriniz..");
                    }
                }

            } while (true);



            if (Fonksiyonlar.sqliteVeritabanim.idDegeriVarMi(id))
            {
                if (Fonksiyonlar.sqliteVeritabanim.SQLiteVeritabanindanVeriSil(id))
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " Id'li item basariyla silinmistir..");
                    ChromeDriverSingleton.yesilYaz(id + " Id'li item basariyla silinmistir..");
                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " Id'li item silinememistir..");
                    ChromeDriverSingleton.kirmiziYaz(id + " Id'li item silinememistir..");
                }
            }
            else
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, id + " idli item veritabaninda bulunmamaktadir..");
                ChromeDriverSingleton.griYaz(id + " idli item veritabaninda bulunmamaktadir..");
            }



        }



        //Veritabanına yeni site eklemeye yarayan fonksiyon.
        public static void yeniSiteEklemeEkrani()
        {
            string siteAdi = "", loginOlduktanSonraBakilacakElement = "", kullaniciAdiEtiketi = "", sifreEtiketi = "", errorEtiketi = "", url = "", errorEtiketiIdMi = "", girisEkraniElementiNameMi = "", servisUrl = "", parametre = "";
            bool errorEtiketiIdMiBool = false, girisEkraniElementiNameMiBool = false;
            int sayac = 0;


            string[] servisUrlleri = new string[500];

            Console.WriteLine();
            Console.WriteLine("-----YENİ SİTE EKLEME EKRANI-----");

            do
            {
                Console.WriteLine("Lütfen eklemek istediğiniz sitenin site adını giriniz: (Örn: arlestest.bimar.com)");
                siteAdi = Console.ReadLine();

                if (String.IsNullOrEmpty(siteAdi))
                {
                    Console.WriteLine("site adi kismi bos gecilemez..");
                }
                else
                {
                    break;
                }

            } while (true);


            do
            {
                Console.WriteLine("Lutfen eklemek istediginiz sitenin url adresini giriniz: (Örn: http://arkasairtest.bimar.com)");
                url = Console.ReadLine();

                if (String.IsNullOrEmpty(url))
                {
                    Console.WriteLine("url bos gecilemez..");
                }
                else
                {
                    break;
                }

            } while (true);

            do
            {
                Console.WriteLine("Lütfen eklemek istediğiniz sitenin hangi parametreye bagli oldugunu giriniz: (Örn: arles)");
                parametre = Console.ReadLine();

                if (String.IsNullOrEmpty(parametre))
                {
                    Console.WriteLine("Parametre degeri bos gecilemez..");
                }
                else
                {
                    break;
                }

            } while (true);

            do
            {
                Console.WriteLine("Lütfen eklemek istediğiniz sitenin (name veya Id attribute'unda olmak kaydiyla)! login sayfasindaki kullanici adi etiketini giriniz: (Örn: name=KullaniciAdiTextBox)");
                kullaniciAdiEtiketi = Console.ReadLine();

                if (String.IsNullOrEmpty(kullaniciAdiEtiketi))
                {
                    Console.WriteLine("kullanici adi etiketi bos gecilemez..");
                }
                else
                {
                    break;
                }

            } while (true);


            do
            {

                Console.WriteLine("Lütfen eklemek istediğiniz sitenin (name veya Id attribute'unda olmak kaydiyla)! login sayfasindaki sifre etiketini giriniz: (Örn: name=SifreTextBox)");
                sifreEtiketi = Console.ReadLine();

                if (String.IsNullOrEmpty(sifreEtiketi))
                {
                    Console.WriteLine("sifre etiketi boş geçilemez..");
                }
                else
                {
                    break;
                }

            } while (true);


            do
            {
                Console.WriteLine("Lutfen eklemek istediginiz sitenin giris ekrani(kullanici adi ve sifre textboxlari) elementlerinin hangi özelliginin aranacagının degerini giriniz. (True ise name'de, False is Id'de arama yapar..)! (Örn: 1:true ya da 0:false)");
                girisEkraniElementiNameMi = Console.ReadLine();

                if (String.IsNullOrEmpty(girisEkraniElementiNameMi))
                {
                    Console.WriteLine("bu deger bos gecilemez..");
                }
                else
                {
                    try
                    {
                        girisEkraniElementiNameMiBool = Convert.ToBoolean(Convert.ToInt32(girisEkraniElementiNameMi));
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("girdiginiz deger tanimlanamadi.. 1 ya da 0 giriniz..");
                    }
                }

            } while (true);


            do
            {
                Console.WriteLine("Lütfen eklemek istediğiniz sitenin (Class veya Id attribute'unda olmak kaydiyla)! login sayfasindaki error etiketini giriniz: (Örn: Id=ErrorInfoLabel)");
                errorEtiketi = Console.ReadLine();

                if (String.IsNullOrEmpty(errorEtiketi))
                {
                    Console.WriteLine("error etiketi bos gecilemez..");
                }
                else
                {
                    break;
                }

            } while (true);


            do
            {
                Console.WriteLine("Lutfen eklemek istediginiz sitenin error etiketinin nerede aranacagının degerini giriniz. True ise Id'de, (False ise Class attribute'unda arama yapar..)! (Örn: 1:true ya da 0:false)");
                errorEtiketiIdMi = Console.ReadLine();

                if (String.IsNullOrEmpty(errorEtiketiIdMi))
                {
                    Console.WriteLine("bu deger bos gecilemez..");
                }
                else
                {
                    try
                    {
                        errorEtiketiIdMiBool = Convert.ToBoolean(Convert.ToInt32(errorEtiketiIdMi));
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("girdiginiz deger tanimlanamadi.. 1 ya da 0 giriniz..");
                    }
                }

            } while (true);

            do
            {
                Console.WriteLine("Lütfen eklemek istediğiniz sitenin login olduktan sonra bakılacak elementinin (Id'sini)! giriniz: (Örn: id=ctl00_UserInfoLabel)");
                loginOlduktanSonraBakilacakElement = Console.ReadLine();

                if (String.IsNullOrEmpty(loginOlduktanSonraBakilacakElement))
                {
                    Console.WriteLine("login olduktan sonra bakilacak element bos gecilemez..");
                }
                else
                {
                    break;
                }
            } while (true);

            
            do
            {
                Console.WriteLine("Lütfen " + siteAdi + " sitesine ait olan web servis urllerini giriniz: (Örn: http://34istmustestapp:8489/)");
                Console.WriteLine("Url girisini bitirmek için 0'a basınız..");
                servisUrl = Console.ReadLine();


                if (String.IsNullOrEmpty(servisUrl))
                {
                    Console.WriteLine("Servis url'i degeri bos gecilemez..");
                }
                else
                {
                    try
                    {

                        if (servisUrl.Equals("0"))
                        {
                            break;
                        }
                        
                        if(sayac== 500)
                        {
                            Console.WriteLine("Maximum url eklenme islemine ulasilmistir..");
                            break;
                        }

                        servisUrlleri[sayac] = servisUrl;
                        sayac++;

                        Console.WriteLine("Eklemek icin url girisine devam edin, girisi bitirmek icin 0'a basınız..");

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Servis url'i degeri tanimlanamadi..");
                    }
                }

            } while (true);

            sayac = 0;




            if (Fonksiyonlar.sqliteVeritabanim.SQLiteVeritabaninaVeriEkle(new WebSitesi(Fonksiyonlar.WebSiteleri.Count, siteAdi, loginOlduktanSonraBakilacakElement, kullaniciAdiEtiketi,
             sifreEtiketi, errorEtiketi, url, errorEtiketiIdMiBool, girisEkraniElementiNameMiBool, servisUrlleri, parametre)))
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, siteAdi + " Web Sitesinin SQLite veritabanina eklenmesi islemi basariyla yapilmistir..");
                ChromeDriverSingleton.yesilYaz(siteAdi + " Web Sitesinin SQLite veritabanina eklenmesi islemi basariyla yapilmistir..");
            }
            else
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, siteAdi + " Web Sitesinin SQLite veritabanina eklenmesi islemi basarisiz olmustur..");
                ChromeDriverSingleton.kirmiziYaz(siteAdi + " Web Sitesinin SQLite veritabanina eklenmesi islemi basarisiz olmustur..");
            }




        }


        //Veritabanındaki kayıtlı olan siteleri görüntüleyen fonksiyon.
        public static void varOlanSiteleriGoruntule()
        {
            Console.WriteLine();

            //Liste boş değilse..
            if (Fonksiyonlar.WebSiteleri.Any())
            {
                Console.WriteLine("Kayitli web siteleri ve özellikleri:");
                Console.WriteLine();
                foreach (WebSitesi item in Fonksiyonlar.WebSiteleri)
                {
                    Console.WriteLine("ID: " + item._ID);
                    Console.WriteLine("Site Adi: " + item._siteAdi);
                    Console.WriteLine("Url: " + item._url);
                    Console.WriteLine("Parametre: " + item._parametre);
                    Console.WriteLine("Id attribute'una bakılmak kaydıyla! Login olduktan sonra anasayfada aranacak elementin id'si: " + item._loginOlduktanSonraBakilacakElement);
                    Console.WriteLine("Name veya Id tipinde olmak kaydıyla kullanici adi etiketi: " + item._kullaniciAdiEtiketi);
                    Console.WriteLine("Sifre etiketi: " + item._sifreEtiketi);
                    Console.WriteLine("Login islemi hatali olursa cikacak hatanin hangi etikete göre taranacagi: " + item._errorEtiketi);
                    Console.WriteLine("girisEkraniElementiNameMi değeri giriş sayfasındaki kullanıcı adı ve sifre textbox degerlerinin neye göre alınacağını belirler. True ise Name'de, False ise Id de arama yapar. İkisi içinde aynıdır.) girisEkraniElementiNameMi değeri: " + item._girisEkraniElementiNameMi);
                    Console.WriteLine("Error etiketinin Id veya Class attribute'una göre aranması errorEtiketiIdMi bool değerine göre bakılır. True ise Id, false ise class'ta arama yapar. errorEtiketiIdMi değeri: " + item._errorEtiketiIdMi);
                    Console.WriteLine();

                    if (item._servisUrlleri != null)
                    {
                        Console.WriteLine(item._siteAdi + " sitesine ait olan servis adresleri: ");
                        for (int i = 0; i < item._servisUrlleri.Length; i++)
                        {
                            Console.WriteLine(item._servisUrlleri[i]);
                        }
                    }
                    else
                    {
                        Console.WriteLine(item._siteAdi + " sitesine ait servis url'i bulunmamaktadir..");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Listede herhangi bir eleman bulunamadi..");
                Console.WriteLine();
            }


        }



    }
}
