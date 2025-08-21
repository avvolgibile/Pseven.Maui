using Pseven.Models;
using Pseven.Varie;

namespace Pseven
{
    public class Statics
    {
        public const int LocalDbTimeout = 400;
        public const string ABSDBFileName = @"C:\Users\User\Documenti\Geniosoft\Maestro Gold\Archivi\P_SUN.gsa";
        public const string JsonPath = @"C:\Users\User\Documenti\Geniosoft\Maestro Gold\Archivi";
        public const string CartellaP_sun = @"C:\Users\User\Documenti\Geniosoft\Maestro Gold\Archivi\P_sun";
        public const string DatiIni = @"C:\Users\User\Documenti\Geniosoft\Maestro Gold\Dati\Dati.ini";
        public const string Excel_per_fattPath1 = @"C:\Users\User\Documenti\Geniosoft\Maestro Gold\Excel\";
        public const string SynfusionLicense = "MzAzMzY0NUAzMjM0MmUzMDJlMzBUbUZmTWVOTVFWdWRNdjAvWmYwbTRXNUx5SXJZemV6TFlBNWF0ZkYyNHZNPQ==";




        public static List<string> SplittaColori(string str, char split)
        {
            List<string> listacolori = [];
            foreach (string c in str.Split(split))
            {
                if(!string.IsNullOrEmpty(c))
                {
                    listacolori.Add(c);
                }
            }
            return listacolori;
        }

#if WINDOWS
        public static Microsoft.UI.Windowing.AppWindow GetAppWindow(MauiWinUIWindow window)
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
            return appWindow;
        }
        //public static MauiWinUIWindow GetParentWindow()
        //{
        //    var window = Shell.Current.GetParentWindow().Handler.PlatformView as MauiWinUIWindow;
        //    return window;
        //}
#endif
    }
}
