using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartIMS.ViewModels
{
    public abstract class BaseClassViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void Initialize();

        #region Alerts
        public async Task SmartIMSAlert(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> SmartIMSAlert(string title, string message, string accept, string cancel)
        {
            bool flag = false;

            flag = await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);

            return flag;
        }

        public async Task<string> SmartIMSActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await App.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }
        #endregion

        public async Task SmartIMSPushAsync(Page page, bool animate = true)
        {
            await (Application.Current.MainPage).Navigation.PushAsync(page);
        }
        public async Task SmartIMSPopAsync()
        {
            await (Application.Current.MainPage).Navigation.PopAsync(true);
        }
        public async Task SmartIMSPopAllPopAsync()
        {
            await (Application.Current.MainPage).Navigation.PopToRootAsync(true);
        }
    }
}