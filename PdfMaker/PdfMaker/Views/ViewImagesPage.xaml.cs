using PdfMaker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PdfMaker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewImagesPage : ContentPage
    {
        public IEnumerable<FileResult> Files;
        readonly BackgroundWorker worker;
        public ViewImagesPage(ref IEnumerable<FileResult> files)
        {
            InitializeComponent();
            Files = files;
            Title = string.Concat($"Viewing {files.Count()} images");
            worker = new();
            worker.DoWork += Worker_DoWork;

        }

        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(async () =>
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    ListView_history.ItemsSource = Files;
                    ListView_history.IsVisible = false;
                });
            });
        }

        //private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    await Task.Run(async () =>
        //     {
        //         try
        //         {
        //             //await MainThread.InvokeOnMainThreadAsync(() =>
        //             await Device.InvokeOnMainThreadAsync(() =>
        //                 {
        //                     if (Convert.ToInt32(e.Argument) != 1)
        //                     {
        //                         stackView.Children.Clear();
        //                     }
        //                     if (Files != null && Files.Any())
        //                     {
        //                         foreach (var item in Files)
        //                         {
        //                             var image = new Image
        //                             {
        //                                 Source = ImageSource.FromFile(item.FullPath),
        //                                 Aspect = Aspect.AspectFit,
        //                             };
        //                             var swipeGesture = new TapGestureRecognizer
        //                             {
        //                                 CommandParameter = item.FileName,
        //                                 NumberOfTapsRequired = 2,
        //                             };
        //                             swipeGesture.Tapped += SwipeGesture_Swiped;
        //                             image.GestureRecognizers.Add(swipeGesture);
        //                             stackView.Children.Add(image);
        //                         }
        //                     }
        //                     if (Convert.ToInt32(e.Argument) == 1)
        //                     {
        //                         LblLoading.IsVisible = false;
        //                     }
        //                     Title = string.Concat($"Viewing {Files.Count()} images");
        //                     refreshView.IsRefreshing = false;
        //                 });
        //         }
        //         catch (Exception ex)
        //         {
        //             await Common.Instance.DisplayAlert(ex.Message);
        //         }
        //     });
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            worker.RunWorkerAsync();
        }

        private void SwipeGesture_Swiped(object sender, EventArgs e)
        {
            //var send = sender as Image;
            string fileName = (e as TappedEventArgs).Parameter as string; //(send.GestureRecognizers.First() as TappedEventArgs).CommandParameter as string;
            Files = Files.Where(s => s.FileName != fileName);

        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            Worker_DoWork(default, default);
        }

        private void ListView_history_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}