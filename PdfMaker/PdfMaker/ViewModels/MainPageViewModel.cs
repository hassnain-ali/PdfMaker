using System;
using System.Collections.Generic;
using System.Text;
using PdfMaker.Services;
using PdfSharpCore;
using Xamarin.Essentials;

namespace PdfMaker
{
    class MainPageViewModel : BaseViewModel
    {
        private PageSize itemSelected;

        public PageSize SelectedItem
        {
            get { return itemSelected; }
            set { SetProperty(ref itemSelected, value); }
        }

        private SensorSpeed sensorSpeedSelected;

        public SensorSpeed SensorSpeedSelected
        {
            get { return sensorSpeedSelected; }
            set { SetProperty(ref sensorSpeedSelected, value); }
        }
        public MainPageViewModel()
        {
            Enum.TryParse(Settings.PdfPageSize, out PageSize pageSize);
            Enum.TryParse(Settings.CancelShakeSpeed, out SensorSpeed sensorSpeed);
            SelectedItem = pageSize;
            SensorSpeedSelected = sensorSpeed;
        }
    }
}
