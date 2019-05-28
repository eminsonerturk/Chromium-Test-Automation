using System;
using System.Globalization;
using System.IO;

namespace LoginSistemi
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            ChromeDriverSingleton.eskiAyaAitDosyalariSil(1);
            
            ChromeDriverSingleton.ePostaArrayiOlustur();
            
            if (args.Length == 0)
            {
                //Argüman yoksa menu ekranına yönlendirir.
                Menu.MenuEkrani();
            }
            else if (args[0].Equals("hepsi"))
            {
               Fonksiyonlar.argumanIleTestEt("");
               ChromeDriverSingleton.getDriverInstance().Quit();
               return;
            }else if (args[0].Contains(".com"))
            {
               Fonksiyonlar.argumanIleTestEt(args[0]);
               ChromeDriverSingleton.getDriverInstance().Quit();
               return;
            }
            else
            {
               Fonksiyonlar.argumanIleTestEt(args[0]);
               ChromeDriverSingleton.getDriverInstance().Quit();
               return;
            }

              
            
        }

        


    }

    

}
