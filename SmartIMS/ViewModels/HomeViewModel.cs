using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartIMS.ClientLib.ClientSDK;
using SmartIMS.ClientLib.Constants;
using SmartIMS.ClientLib.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartIMS.ViewModels
{
    public class HomeViewModel : BaseClassViewModel
    {
        public HomeViewModel() : base() { Initialize(); }
        public override void Initialize()
        {
            _titleText = StringConstants.HomeScreen_Title;
            _browseButtonText = StringConstants.HomeScreen_BrowseButtonText;
            _clearButtonText = StringConstants.HomeScreen_ClearButtonText;
        }

        private string _titleText;
        private string _browseButtonText;
        private string _clearButtonText;
        public List<TotalTrips> _totalTripsList;
        public List<TotalTrips> TotalTripsList
        {
            get => _totalTripsList;
            set => SetProperty(ref _totalTripsList, value);
        }
        private async Task PickAndShow()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> { { DevicePlatform.Android, new[] { "text/*" } }, });
            var options = new PickOptions
            {
                PickerTitle = StringConstants.HomeScreen_FilePickerTitle,
                FileTypes = customFileType,
            };

            var result = await FilePicker.PickAsync(options);
            if (result != null)
            {

                if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                {
                    var stream = await result.OpenReadAsync();
                    using (var objTripsProcessing = new TripsProcessing())
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        string commandLine;
                        while ((commandLine = reader.ReadLine()) != null)
                            objTripsProcessing.ProcessCommandLine(commandLine);

                        TotalTripsList = objTripsProcessing.GetTotalTripsList();
                    }
                }
            }
        }


        public string TitleText
        {
            get => _titleText;
            set { SetProperty(ref _titleText, value); }
        }
        public string BrowseButtonText
        {
            get => _browseButtonText;
            set { SetProperty(ref _browseButtonText, value); }
        }
        public string ClearButtonText
        {
            get => _clearButtonText;
            set { SetProperty(ref _clearButtonText, value); }
        }
        public ICommand onBrowseFileCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await PickAndShow();
                        }
                        catch (Exception ex)
                        {
                            await SmartIMSAlert("Text", ex.Message, "OK");
                        }
                    });
                });
            }
        }

        public ICommand onClearCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            TotalTripsList = new List<TotalTrips>();
                        }
                        catch (Exception ex)
                        {
                            await SmartIMSAlert("Text", ex.Message, "OK");
                        }
                    });
                });
            }
        }
    }
}
