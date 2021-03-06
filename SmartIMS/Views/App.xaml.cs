using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartIMS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetCultureToUSEnglish();
            MainPage = new Home();
        }

        void SetCultureToUSEnglish()
        {
            CultureInfo englishUSCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishUSCulture;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
