using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;




namespace LoginSistemi
{
    public class SqliteAccessInterface
    {
        
        public string connectionString = "Data Source = "+ ChromeDriverSingleton.veritabaniPath;

        public SQLiteConnection SqliteConnection { get; set; }
        
        public SQLiteCommand SqlCommand { get; set; }

        //Data table class dizisinin 1. dizisi web sitesi bilgilerini tutarken, 2. dizisi web sitesi url bilgilerini tutmaktadir.. 
        public DataTable[] DataTable { get; set; }


        public SQLiteDataAdapter dataAdapter { get; set; }

        public SqliteAccessInterface()
        {
            SqliteConnection = new SQLiteConnection();
            DataTable = new DataTable[2];

        }
        
        //Veritabanına bağlanmayı sağlayan fonksiyon.
        public bool VeritabaninaBaglan()
        {

            try
            {
                SqliteConnection.ConnectionString = connectionString;
                SqliteConnection.Open();
                return true;
            }
            catch (Exception)
            {
                try
                {
                    SqliteConnection = new SQLiteConnection();
                    SqliteConnection.ConnectionString = connectionString;
                    return true;
                }
                catch (Exception ex)
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath,"SQLite baglantisi kurulamadi..  Hata:" + ex.ToString());
                    ChromeDriverSingleton.kirmiziYaz("SQLite baglantisi kurulamadi..");
                    return false;
                }
                
            }
            
        }

        
        //Veritabanı bağlantısını kesmeye yarayan fonksiyon.
        public bool VeritabaniBaglantisiniKes()
        {

            try
            {
                SqliteConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veritabani baglantisi sonlandirilamadi..  Hata: " + ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("Veritabani baglantisi sonlandirilamadi..");
                return false;
            }

        }

        //DataAdapter yaratmaya yarayan fonksiyon.
        public SQLiteDataAdapter sqliteDataAdapterYarat(SQLiteCommand gelenCommand)
        {
            try
            {
                return new SQLiteDataAdapter(gelenCommand);
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SqliteDataAdapter yaratilamadi..  Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("SqliteDataAdapter yaratilamadi..");
                return null;
            }
            
        }
        
        //Sorgu commentini getirmeye yarayan fonksiyon.
        public SQLiteCommand sorguCommentiniGetir(string sqlSorgusu, SQLiteConnection baglan)
        {
            try
            {
                if (VeritabaninaBaglan())
                {
                    return new SQLiteCommand(sqlSorgusu, baglan);
                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SQLiteCommand olusturulamadi..");
                    ChromeDriverSingleton.kirmiziYaz("SQLiteCommand olusturulamadi..");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SQLiteCommand olusturulamadi.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("SQLiteCommand oluşturulamadi..");
                return null;
            }
        }

        //Veritabanındaki tüm verileri getirmeyi sağlayan fonksiyon.
        public void TumVerileriGetir()
        {

            try
            {
                if(DataTable[0] != null)
                {
                    DataTable[0].Clear();
                }else
                {
                    DataTable[0] = new DataTable();
                }

                if(DataTable[1] != null)
                {
                    DataTable[1].Clear();
                }else
                {
                    DataTable[1] = new DataTable();
                }

                SqlCommand = sorguCommentiniGetir("SELECT * FROM webSiteleri", SqliteConnection);

                if(SqlCommand != null)
                {
                    dataAdapter = sqliteDataAdapterYarat(SqlCommand);

                    if(dataAdapter != null)
                    {
                        dataAdapter.Fill(DataTable[0]);
                    }
                    else
                    {
                        Console.WriteLine("Web Sitelerine ait veritabani tarafinda dataAdapter oluşturulamadi..");
                    }
                    
                }
                else
                {
                    Console.WriteLine("Web sitelerine ait veriler okunamadi..");
                }


                SqlCommand = sorguCommentiniGetir("SELECT * FROM servisUrlleri", SqliteConnection);

                if (SqlCommand != null)
                {
                    dataAdapter = sqliteDataAdapterYarat(SqlCommand);
                    
                    if (dataAdapter != null)
                    {
                        dataAdapter.Fill(DataTable[1]);
                    }
                    else
                    {
                        Console.WriteLine("Web sitesi servis urllerine ait veritabani tarafinda dataAdapter olusturulamadi..");
                    }

                    
                }
                else
                {
                    Console.WriteLine("Web sitesi servis urllerine ait veriler okunamadi..");
                }


                VeritabaniBaglantisiniKes();
                
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SELECT * FROM webSiteleri sorgusuna ait dataTable bulunamadi.. Hata: " + ex.ToString());
            }
        
        }

        //İlgili sorguya ait değerkeri getirmeyi sağlayan fonksiyon.
        public DataTable ilgiliSorguyaDairWebSitesiDegerleriniGetir(string sqlSorgusu)
        {
            try
            {
                if (DataTable[0] != null)
                {
                    DataTable[0].Clear();
                }
                else
                {
                    DataTable[0] = new DataTable();
                }

                SqlCommand = sorguCommentiniGetir(sqlSorgusu, SqliteConnection);

                if (SqlCommand != null)
                {
                    dataAdapter = sqliteDataAdapterYarat(SqlCommand);

                    if (dataAdapter != null)
                    {
                        dataAdapter.Fill(DataTable[0]);
                    }
                    else
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Web Sitelerine ait veritabani tarafinda dataAdapter oluşturulamadi..");
                        ChromeDriverSingleton.kirmiziYaz("Web Sitelerine ait veritabani tarafinda dataAdapter oluşturulamadi..");
                    }

                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Web sitelerine ait veriler okunamadi..");
                    ChromeDriverSingleton.kirmiziYaz("Web sitelerine ait veriler okunamadi..");
                }


                if (VeritabaniBaglantisiniKes())
                {
                    return DataTable[0];
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, sqlSorgusu + " sorgusuna ait veri bulunamadi.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.griYaz(sqlSorgusu+" sorgusuna ait veri bulunamadi.. Hata:" + ex.ToString());
                return null;
            }
        }

        //İlgili sorguya ait web sitesi url değerlerini getirmeyi sağlayan fonksiyon.
        public DataTable ilgiliSorguyaDairWebSitesiUrlDegerleriniGetir(string sqlSorgusu)
        {
            try
            {
                if (DataTable[1] != null)
                {
                    DataTable[1].Clear();
                }
                else
                {
                    DataTable[1] = new DataTable();
                }

                SqlCommand = sorguCommentiniGetir(sqlSorgusu, SqliteConnection);

                if (SqlCommand != null)
                {
                    dataAdapter = sqliteDataAdapterYarat(SqlCommand);

                    dataAdapter = sqliteDataAdapterYarat(SqlCommand);

                    if (dataAdapter != null)
                    {
                        dataAdapter.Fill(DataTable[1]);
                    }
                    else
                    {
                        ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Web sitesi servis urllerine ait veritabani tarafinda dataAdapter olusturulamadi..");
                        ChromeDriverSingleton.kirmiziYaz("Web sitesi servis urllerine ait veritabani tarafinda dataAdapter olusturulamadi..");
                    }


                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Web sitesi servis urllerine ait veriler okunamadi..");
                    ChromeDriverSingleton.kirmiziYaz("Web sitesi servis urllerine ait veriler okunamadi..");
                }


                if (VeritabaniBaglantisiniKes())
                {
                    return DataTable[1];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, sqlSorgusu + " sorgusuna ait veri bulunamadi.. Hata:" + ex.ToString());
                ChromeDriverSingleton.griYaz(sqlSorgusu + " sorgusuna ait veri bulunamadi.. Hata: "+ ex.ToString());
                return null;
            }
        }

        //İlgili sorguyu çalıştırmayı sağlayan fonksiyon.
        public bool IlgiliSorguyuCalistir(string sqlSorgusu)
        {
            try
            {
                SqlCommand = sorguCommentiniGetir(sqlSorgusu, SqliteConnection);

                SqlCommand.ExecuteNonQuery();

                if (VeritabaniBaglantisiniKes())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, sqlSorgusu + " SQLite tarafindan calistirilamadi.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz(sqlSorgusu + " SQLite tarafindan calistirilamadi.. Hata: "+ ex.ToString());
                return false;
            }

        }

        //Veritabanında verilen id'li değerin olup olmadığını sorgulayan fonksiyon.
        public bool idDegeriVarMi(int ID)
        {
            try
            {
                if (DataTable[0] != null) {
                    DataTable[0].Clear();
                }
                else{
                    DataTable[0] = new DataTable();
                }

                SqlCommand = sorguCommentiniGetir("SELECT * FROM webSiteleri WHERE ID=@ID", SqliteConnection);


                SQLiteParameter prm1 = new SQLiteParameter("ID", ID);

                SqlCommand.Parameters.Add(prm1);

                dataAdapter = sqliteDataAdapterYarat(SqlCommand);
                dataAdapter.Fill(DataTable[0]);


                VeritabaniBaglantisiniKes();
                

                if(DataTable[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, " Veritabanından datalar çekilirken hata meydana geldi.. Hata: " + ex.ToString());
                ChromeDriverSingleton.griYaz(" Veritabanından datalar çekilirken hata meydana geldi.. Hata: " + ex.ToString());
                return false;
            }
        }


        //Sqlite veritabanına veri eklemeyi sağlayan fonksiyon.
        public bool SQLiteVeritabaninaVeriEkle(WebSitesi gelenWebSitesiElementi)
        {
            int errorDegeri = 0, girisEkraniDegeri = 0, sonKisimdakiIdDegeri = 0;
            string sqlWebSitesi, sqlServisUrlleri;

            try
            {
                if (gelenWebSitesiElementi._errorEtiketiIdMi)
                {
                    errorDegeri = 1;
                }
                else
                {
                    errorDegeri = 0;
                }
                if (gelenWebSitesiElementi._girisEkraniElementiNameMi)
                {
                    girisEkraniDegeri = 1;
                }
                else
                {
                    girisEkraniDegeri = 0;
                }

                
                SQLiteParameter prm1 = new SQLiteParameter("siteAdi", gelenWebSitesiElementi._siteAdi);
                SQLiteParameter prm2 = new SQLiteParameter("loginOlduktanSonraBakilacakElement", gelenWebSitesiElementi._loginOlduktanSonraBakilacakElement);
                SQLiteParameter prm3 = new SQLiteParameter("kullaniciAdiEtiketi", gelenWebSitesiElementi._kullaniciAdiEtiketi);
                SQLiteParameter prm4 = new SQLiteParameter("sifreEtiketi", gelenWebSitesiElementi._sifreEtiketi);
                SQLiteParameter prm5 = new SQLiteParameter("errorEtiketi", gelenWebSitesiElementi._errorEtiketi);
                SQLiteParameter prm6 = new SQLiteParameter("url", gelenWebSitesiElementi._url);
                SQLiteParameter prm7 = new SQLiteParameter("errorEtiketiIdMi", errorDegeri);
                SQLiteParameter prm8 = new SQLiteParameter("girisEkraniElementiNameMi", girisEkraniDegeri);
                SQLiteParameter prm9 = new SQLiteParameter("parametre", gelenWebSitesiElementi._parametre);


                sqlWebSitesi = "INSERT INTO webSiteleri (siteAdi, loginOlduktanSonraBakilacakElement,kullaniciAdiEtiketi, sifreEtiketi, errorEtiketi, url, errorEtiketiIdMi, girisEkraniElementiNameMi, parametre) "
                    + "VALUES (@siteAdi, @loginOlduktanSonraBakilacakElement, @kullaniciAdiEtiketi, @sifreEtiketi, @errorEtiketi , @url, @errorEtiketiIdMi, @girisEkraniElementiNameMi, @parametre)";

                SqlCommand = sorguCommentiniGetir(sqlWebSitesi, SqliteConnection);

                if(SqlCommand != null)
                {
                    SqlCommand.Parameters.Add(prm1);
                    SqlCommand.Parameters.Add(prm2);
                    SqlCommand.Parameters.Add(prm3);
                    SqlCommand.Parameters.Add(prm4);
                    SqlCommand.Parameters.Add(prm5);
                    SqlCommand.Parameters.Add(prm6);
                    SqlCommand.Parameters.Add(prm7);
                    SqlCommand.Parameters.Add(prm8);
                    SqlCommand.Parameters.Add(prm9);
                }

                SqlCommand.ExecuteNonQuery();


                VeritabaniBaglantisiniKes();


                DataTable[0] = ilgiliSorguyaDairWebSitesiDegerleriniGetir("SELECT MAX(ID) AS ID FROM webSiteleri");

                //Son eklenen degerin id numarasini veren islem..
                if (DataTable[0].Rows.Count > 0)
                    sonKisimdakiIdDegeri = Convert.ToInt32(DataTable[0].Rows[0]["ID"]);
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, sonKisimdakiIdDegeri + " id'li veri, veritabaninda deger bulunamadi..");
                    ChromeDriverSingleton.griYaz("Veritabaninda deger bulunamadi..");
                    return false;
                }

                

                sqlServisUrlleri = "INSERT INTO servisUrlleri (ID, servisUrl) VALUES (@ID, @servisUrl)";
                
                if (SqlCommand != null)
                {
                    for (int i = 0; i < gelenWebSitesiElementi._servisUrlleri.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(gelenWebSitesiElementi._servisUrlleri[i]))
                        {
                            
                            SqlCommand = sorguCommentiniGetir(sqlServisUrlleri, SqliteConnection);
                            SQLiteParameter prm10 = new SQLiteParameter("ID", sonKisimdakiIdDegeri);
                            SQLiteParameter prm11 = new SQLiteParameter("servisUrl", gelenWebSitesiElementi._servisUrlleri[i]);

                            SqlCommand.Parameters.Add(prm10);
                            SqlCommand.Parameters.Add(prm11);

                            SqlCommand.ExecuteNonQuery();

                            VeritabaniBaglantisiniKes();
                        }
                        else
                        {
                            break;
                        }
                    }
                }


                return true;
                
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SQLite veritabanina veri eklerken bir problem oluştu.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("SQLite veritabanina veri eklerken bir problem oluştu.. Hata: "+ ex.ToString());
                return false;
            }
            

        }

        //Sqlite veritabanından veri silmeyi sağlayan fonksiyon.
        public bool SQLiteVeritabanindanVeriSil(int ID)
        {
            try
            {
                string webSitesiSql = "DELETE FROM webSiteleri WHERE ID = @ID;";

                SQLiteParameter prm1 = new SQLiteParameter("ID", ID);
                
                SqlCommand = sorguCommentiniGetir(webSitesiSql, SqliteConnection);

                SqlCommand.Parameters.Add(prm1);
                
                SqlCommand.ExecuteNonQuery();
                
                VeritabaniBaglantisiniKes();


                string webSitesiServisUrlSql = "DELETE FROM servisUrlleri WHERE ID = @ID;";

                SQLiteParameter prm2 = new SQLiteParameter("ID", ID);

                SqlCommand = sorguCommentiniGetir(webSitesiServisUrlSql, SqliteConnection);

                SqlCommand.Parameters.Add(prm2);

                SqlCommand.ExecuteNonQuery();

                VeritabaniBaglantisiniKes();

                return true;

            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SQLite veritabanindan veri silinirken bir problem oluştu.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("SQLite veritabanindan veri silinirken bir problem oluştu.. Hata: "+ ex.ToString());
                return false;
            }
        }


        //Sqlite veritabanındaki veriyi düzeltmeyi sağlayan fonksiyon.
        public bool SQLiteVeritabanindakiVeriyiDüzelt(int ID, string duzenlenecekParametre, string duzenlenecekParametreninDegeri)
        {
            try
            {

                string sql = "UPDATE webSiteleri SET "+duzenlenecekParametre+"=@duzenlenecekParametreninDegeri WHERE ID = @ID";
                
                SQLiteParameter prm2 = new SQLiteParameter("duzenlenecekParametreninDegeri", duzenlenecekParametreninDegeri);
                SQLiteParameter prm3 = new SQLiteParameter("ID", ID);


                SqlCommand = sorguCommentiniGetir(sql, SqliteConnection);
                
                SqlCommand.Parameters.Add(prm2);
                SqlCommand.Parameters.Add(prm3);

                SqlCommand.ExecuteNonQuery();

                VeritabaniBaglantisiniKes();

                return true;
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veri tabaninda, " + ID +" id'li internet sitesi degerine ait olan "+ duzenlenecekParametre + " degeri düzeltilememistir.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("Veri tabaninda, " + duzenlenecekParametre + " degeri düzeltilememistir.. Hata: "+ ex.ToString());
                return false;
            }
            
        }

        //Verilen site adlarına göre değerlerin alınmasını sağlayan fonksiyon.
        public bool parametreAdinaGoreDegerleriAl(string parametre)
        {
            DataTable alinanDeger = new DataTable();
            
            try
            {
                string sqlSorgusu = "";
                if (parametre.Equals(""))
                {
                    sqlSorgusu = "SELECT * FROM webSiteleri WHERE siteAdi LIKE '%%';";
                }else if (parametre.Contains(".com"))
                {
                    sqlSorgusu = "SELECT * FROM webSiteleri WHERE siteAdi LIKE '%"+ parametre +"%';";
                }
                else
                {
                    sqlSorgusu = "SELECT * FROM webSiteleri WHERE parametre LIKE '%" + parametre + "%';";
                }
                

                ilgiliSorguyaDairWebSitesiDegerleriniGetir(sqlSorgusu);

                for (int i = 0; i < DataTable[0].Rows.Count; i++)
                {

                    ilgiliSorguyaDairWebSitesiUrlDegerleriniGetir("SELECT * FROM servisUrlleri WHERE ID = " + DataTable[0].Rows[i]["ID"]);

                    int deger = DataTable[1].Rows.Count;

                    if (DataTable[1] != null)
                    {
                        alinanDeger.Merge(DataTable[1]);
                        DataTable[1].Clear();
                    }
                    else
                    {
                        DataTable[1] = new DataTable();
                    }

                }
                
                DataTable[1] = alinanDeger;


                int a1 = DataTable[0].Rows.Count;
                int a2 = DataTable[1].Rows.Count;
                

                if (dataTabledakiDegerleriListeyeAta(DataTable[0], alinanDeger))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            

            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "SQLite veritabanindan " + parametre + " parametre degerine ait web siteleri alinirken bir problem olustu..) Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("SQLite veritabanindan " + parametre + " parametre degerine ait web siteleri alinirken bir problem olustu..) Hata: "+ ex.ToString());
                return false;
            }


        }


        //Veritabanındaki servis urlleri değerlerini düzenlemeyi sağlayan fonksiyon.
        public bool SQLiteVeritabanindakiVeriyiDüzelt(int id, string[] servisUrlleri)
        {
            try
            {
                
                string silmeSqliSorgusu = "DELETE FROM servisUrlleri WHERE ID = @ID";
                string servisUrliEklemeSorgusu = "INSERT INTO servisUrlleri VALUES (@ID, @servisUrli)";

                SqlCommand = sorguCommentiniGetir(silmeSqliSorgusu, SqliteConnection);

                if(SqlCommand != null)
                {
                    SQLiteParameter prm = new SQLiteParameter("ID", id);

                    SqlCommand.Parameters.Add(prm);
                    
                    SqlCommand.ExecuteNonQuery();

                    VeritabaniBaglantisiniKes();
                }
                
                
                if (!(string.IsNullOrEmpty(servisUrlleri[0])) && servisUrlleri != null)
                {
                    for (int i = 0; i < servisUrlleri.Length; i++)
                    {

                        if (!(string.IsNullOrEmpty(servisUrlleri[i]))){

                            SqlCommand = sorguCommentiniGetir(servisUrliEklemeSorgusu, SqliteConnection);
                            SQLiteParameter prm = new SQLiteParameter("ID", id);
                            SQLiteParameter prm2 = new SQLiteParameter("servisUrli", servisUrlleri[i]);

                            SqlCommand.Parameters.Add(prm);
                            SqlCommand.Parameters.Add(prm2);

                            SqlCommand.ExecuteNonQuery();

                            VeritabaniBaglantisiniKes();
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                }
                else
                {
                    Console.WriteLine("Duzenlenecek parametre girisi yapilmamis..");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veritabaninda, " + id + " id degerine sahip olan servisUrlleri degeri düzeltilememistir.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("Veritabaninda, servisUrlleri degeri düzeltilememistir.. Hata: "+ ex.ToString());
                return false;
            }
        }





        //Sqlite veri tabanındaki verileri düzenlemeyi sağlayan fonksiyon.
        public bool SQLiteVeritabanindakiVeriyiDüzelt(int ID, string duzenlenecekParametre, bool duzenlenecekParametreninDegeri)
        {
            try
            {
                int duzenlenecekParametreninIntDegeri = 0;

                if (duzenlenecekParametreninDegeri)
                {
                    duzenlenecekParametreninIntDegeri = 1;
                }


                string sql = "UPDATE webSiteleri SET "+ duzenlenecekParametre+"=@duzenlenecekParametreninDegeri WHERE ID = @ID";
                
                SQLiteParameter prm2 = new SQLiteParameter("duzenlenecekParametreninDegeri", duzenlenecekParametreninIntDegeri);
                SQLiteParameter prm3 = new SQLiteParameter("ID", ID);


                SqlCommand = sorguCommentiniGetir(sql, SqliteConnection);

                SqlCommand.Parameters.Add(prm2);
                SqlCommand.Parameters.Add(prm3);

                SqlCommand.ExecuteNonQuery();

                VeritabaniBaglantisiniKes();

                return true;
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veritabaninda, " + ID + " id degerine sahip olan " + duzenlenecekParametre + " degeri düzeltilememistir.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("Veri tabaninda, " + duzenlenecekParametre + " degeri düzeltilememistir.. Hata: "+ ex.ToString());
                return false;
            }
        }

        //Tanımlanan dataTable'daki değerleri listeye atamayı sağlayan fonksiyon.
        public bool dataTabledakiDegerleriListeyeAta()
        {
            List<string> servisUrllerim = new List<string>();
            string[] servisUrllerimArray = null;

            if(DataTable[0] != null){
                WebSitesi[] websiteBilgileri = new WebSitesi[DataTable[0].Rows.Count];

                if (DataTable[0].Rows.Count > 0)
                {
                    for (int j = 0; j < DataTable[0].Rows.Count; j++)
                    {

                        websiteBilgileri[j] = new WebSitesi(Convert.ToInt32(DataTable[0].Rows[j].ItemArray[0]), Convert.ToString(DataTable[0].Rows[j].ItemArray[1]), Convert.ToString(DataTable[0].Rows[j].ItemArray[2]),
                        Convert.ToString(DataTable[0].Rows[j].ItemArray[3]), Convert.ToString(DataTable[0].Rows[j].ItemArray[4]), Convert.ToString(DataTable[0].Rows[j].ItemArray[5]), Convert.ToString(DataTable[0].Rows[j].ItemArray[6]), Convert.ToBoolean(DataTable[0].Rows[j].ItemArray[7]),
                       Convert.ToBoolean(DataTable[0].Rows[j].ItemArray[8]), null, Convert.ToString(DataTable[0].Rows[j].ItemArray[9]));


                        if (DataTable[1] != null)
                        {
                            //Url alma işlemleri burada yapılacaktır..
                            if (DataTable[1].Rows.Count > 0)
                            {
                                
                                for (int a = 0; a < DataTable[1].Rows.Count; a++)
                                {
                                    if(Convert.ToInt32(DataTable[0].Rows[j].ItemArray[0]) == Convert.ToInt32(DataTable[1].Rows[a].ItemArray[0])){

                                        servisUrllerim.Add(Convert.ToString(DataTable[1].Rows[a].ItemArray[1]));

                                    }


                                }


                                if (servisUrllerim.Any())
                                {
                                    servisUrllerimArray = new string[servisUrllerim.Count()];

                                    for (int i = 0; i < servisUrllerim.Count(); i++)
                                    {
                                        servisUrllerimArray[i] = servisUrllerim.ElementAt(i);
                                    }


                                    if(servisUrllerimArray[0] != null)
                                        websiteBilgileri[j]._servisUrlleri = servisUrllerimArray;


                                    if (servisUrllerim.Any())
                                        servisUrllerim.Clear();
                                }
                                
                               
                                
                            }

                        }
                        else{
                            ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veritabaninda web sitelerine ait url degerleri bulunmamaktadir..");
                            ChromeDriverSingleton.griYaz("Veritabaninda web sitelerine ait url degerleri bulunmamaktadir..");
                        }


                    }


                    if (Fonksiyonlar.WebSiteleri.Any())
                        Fonksiyonlar.WebSiteleri.Clear();

                    for (int a = 0; a < websiteBilgileri.Length; a++)
                    {
                        Fonksiyonlar.WebSiteleri.Add(websiteBilgileri[a]);
                    }


                    return true;

                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veritabaninda herhangi bir bilgi bulunmamaktadir..");
                    ChromeDriverSingleton.griYaz("Veritabaninda herhangi bir bilgi bulunmamaktadir..");
                    return false;
                }

            }



            return true;

        }

        //Verilen dataTable'daki değerleri listeye atayan fonksiyon.
        public bool dataTabledakiDegerleriListeyeAta(DataTable dataTable1, DataTable dataTable2)
        {
            List<string> servisUrllerim = new List<string>();
            string[] servisUrllerimArray = null;

            if (dataTable1 != null)
            {
                WebSitesi[] websiteBilgileri = new WebSitesi[dataTable1.Rows.Count];

                if (dataTable1.Rows.Count > 0)
                {
                    for (int j = 0; j < dataTable1.Rows.Count; j++)
                    {

                        websiteBilgileri[j] = new WebSitesi(Convert.ToInt32(dataTable1.Rows[j].ItemArray[0]), Convert.ToString(dataTable1.Rows[j].ItemArray[1]), Convert.ToString(dataTable1.Rows[j].ItemArray[2]),
                        Convert.ToString(dataTable1.Rows[j].ItemArray[3]), Convert.ToString(dataTable1.Rows[j].ItemArray[4]), Convert.ToString(dataTable1.Rows[j].ItemArray[5]), Convert.ToString(dataTable1.Rows[j].ItemArray[6]), Convert.ToBoolean(dataTable1.Rows[j].ItemArray[7]),
                       Convert.ToBoolean(dataTable1.Rows[j].ItemArray[8]), null, Convert.ToString(DataTable[0].Rows[j].ItemArray[9]));


                        if (dataTable2 != null)
                        {
                            //Url alma işlemleri burada yapılacaktır..
                            if (dataTable2.Rows.Count > 0)
                            {

                                for (int a = 0; a < dataTable2.Rows.Count; a++)
                                {
                                    if (Convert.ToInt32(dataTable1.Rows[j].ItemArray[0]) == Convert.ToInt32(dataTable2.Rows[a].ItemArray[0]))
                                    {

                                        servisUrllerim.Add(Convert.ToString(dataTable2.Rows[a].ItemArray[1]));

                                    }


                                }


                                if (servisUrllerim.Any())
                                {
                                    servisUrllerimArray = new string[servisUrllerim.Count()];

                                    for (int i = 0; i < servisUrllerim.Count(); i++)
                                    {
                                        servisUrllerimArray[i] = servisUrllerim.ElementAt(i);
                                    }


                                    if (servisUrllerimArray[0] != null)
                                        websiteBilgileri[j]._servisUrlleri = servisUrllerimArray;


                                    if (servisUrllerim.Any())
                                        servisUrllerim.Clear();
                                }



                            }

                        }
                        else
                        {
                            Console.WriteLine("Veritabaninda web sitelerine ait url degerleri bulunmamaktadir..");
                        }


                    }


                    if (Fonksiyonlar.WebSiteleri.Any())
                        Fonksiyonlar.WebSiteleri.Clear();

                    for (int a = 0; a < websiteBilgileri.Length; a++)
                    {
                        Fonksiyonlar.WebSiteleri.Add(websiteBilgileri[a]);
                    }


                    return true;

                }
                else
                {
                    ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Veritabaninda herhangi bir bilgi bulunmamaktadir..");
                    ChromeDriverSingleton.griYaz("Veritabaninda herhangi bir bilgi bulunmamaktadir..");
                    return false;
                }

            }
            
            return true;
        }

        //Veritabanı değerlerini listeye atayan fonksiyon.
        public bool veritabaniDegerleriniListeyeAta()
        {
            try
            {
                
                TumVerileriGetir();

                return dataTabledakiDegerleriListeyeAta();

                
            }
            catch (Exception ex)
            {
                ChromeDriverSingleton.dosyayaCumleYaz(ChromeDriverSingleton.logDosyasiPath, "Web siteleri SQLitedan atanırken bir problem olustu.. Hata: "+ ex.ToString());
                ChromeDriverSingleton.kirmiziYaz("Web siteleri SQLitedan atanırken bir problem olustu.. Hata: "+ ex.ToString());
                return false;
            }
        }


        

        



        


    }
}
