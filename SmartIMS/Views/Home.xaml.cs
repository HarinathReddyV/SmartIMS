using SmartIMS.ViewModels;
using Xamarin.Forms;

namespace SmartIMS
{
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            this.BindingContext = new HomeViewModel();
        }
    }
}
