using Microsoft.Maui.Devices;
using Pseven.Views;
using Pseven.ViewModels;
using Pseven.Data;
using System.Globalization;
using Pseven.Controls;

namespace Pseven
{

    //Server=tcp:psevenserver.database.windows.net,1433;Initial Catalog=PsevenSunDB;Persist Security Info=False;User ID=adminuserpseven;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;


    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Statics.SynfusionLicense);
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

            window.Width = displayInfo.Width / 1.8;
            window.Height = displayInfo.Height / 1.8;
            window.MinimumHeight = window.Height;
            window.MinimumWidth = window.Width;
            window.X = ((displayInfo.Width / displayInfo.Density) - window.Width) / 2;
            window.Y = ((displayInfo.Height / displayInfo.Density) - window.Height) / 2;
            
            return window;
        }
    }
}
