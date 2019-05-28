namespace LoginSistemi
{
    public class WebSitesi
    {
        public int _ID { get; set; }
        public string _siteAdi { get; set; }
        public string _loginOlduktanSonraBakilacakElement { get; set; }
        public string _kullaniciAdiEtiketi { get; set; }
        public string _sifreEtiketi { get; set; }
        public string _errorEtiketi { get; set; }
        public string _url { get; set; }
        public bool _errorEtiketiIdMi { get; set; }
        public bool _girisEkraniElementiNameMi { get; set; }

        public string _parametre { get; set; }

        public string[] _servisUrlleri { get; set; }

        public WebSitesi(int ID, string siteAdi, string loginOlduktanSonraBakilacakElement, string kullaniciAdiEtiketi, string sifreEtiketi, string errorEtiketi,
           string url, bool errorEtiketiIdMi, bool girisEkraniElementiNameMi, string[] servisUrlleri, string parametre)
        {
            _ID = ID;
            _siteAdi = siteAdi;
            _loginOlduktanSonraBakilacakElement = loginOlduktanSonraBakilacakElement;
            _kullaniciAdiEtiketi = kullaniciAdiEtiketi;
            _sifreEtiketi = sifreEtiketi;
            _errorEtiketi = errorEtiketi;
            _url = url;
            _errorEtiketiIdMi = errorEtiketiIdMi;
            _girisEkraniElementiNameMi = girisEkraniElementiNameMi;
            _servisUrlleri = servisUrlleri;
            _parametre = parametre;

        }


    }
}
