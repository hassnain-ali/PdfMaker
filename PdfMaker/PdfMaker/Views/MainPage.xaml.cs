using Android.Widget;
using PdfMaker.Data;
using PdfMaker.Models;
using PdfMaker.Services;
using PdfMaker.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using static PdfMaker.Directories;

namespace PdfMaker
{
    public partial class MainPage : ContentPage
    {
        public IEnumerable<FileResult> Files { get; set; }
        private CancellationToken cancellationToken;
        CancellationTokenSource src;
        readonly Stopwatch stopwatch;
        private readonly Timer timer;
        public MainPage()
        {
            InitializeComponent();
            stopwatch = new();
            timer = new(TimerChanged, null, 0, 1000);
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            if (!Accelerometer.IsMonitoring)
            {
                Enum.TryParse(Settings.CancelShakeSpeed, out SensorSpeed speed);
                Accelerometer.Start(speed);
            }
        }

        private async void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            if (Files != null && Files.Any())
            {
                if (await Common.Instance.DisplayAlert("You want to clear all choices?", true))
                {
                    Files = Enumerable.Empty<FileResult>();
                    UnBlockUi();
                    StopIndicator();
                    StopTimer();
                    BtnCancel_Clicked(default, default);
                }
            }
        }

        private async void BtnSelect_Clicked(object sender, EventArgs e)
        {
            try
            {
                Files = await FilePicker.PickMultipleAsync(new() { FileTypes = FilePickerFileType.Images });
                if (Files != null && Files.Any())
                {
                    Files = Files.OrderBy(s => s.FileName);
                    BlockUi();
                }
                else
                {
                    UnBlockUi();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Something went wrong", ex.Message, "Ok");
            }
        }

        private async void BtnMake_Clicked(object sender, EventArgs e)
        {
            try
            {
                StartTimer();
                src = new CancellationTokenSource();
                cancellationToken = src.Token;
                RunIndicator();
                BtnCancel.IsEnabled = true;
                string root = GetOrCreateRoot();
                string path = SafeFileName(Path.Combine(root, PdfFileName(TxtFileName.Text)));
                var pdfMaker = new PDFMaker();
                IEnumerable<FilesResult> newFiles = await Common.Instance.GetResizedFileNames(Files);
                var document = await pdfMaker.CreatePdf(newFiles, cancellationToken);
                if (document != null)
                {
                    document.Save(path);
                    var db = await HistoryDatabaseContext.Instance;
                    await db.SaveItemAsync(new()
                    {
                        FilePath = path,
                        FileName = Path.GetFileNameWithoutExtension(path),
                        CreatedDate = DateTime.Now,
                    });
                    RefreshRoots(new[] { root });
                    RefreshRoots(new[] { path });
                    UnBlockUi();
                    StopIndicator();
                    StopTimer();
                    Files = Enumerable.Empty<FileResult>();
                    if (Settings.DefaultOpenFileAfterMake)
                    {
                        await Launcher.OpenAsync(new OpenFileRequest() { File = new ReadOnlyFile(path) });
                    }
                    else
                    {
                        Common.Instance.MakeToast($"Created File '{Path.GetFileNameWithoutExtension(path)}'", ToastLength.Long);
                    }
                }
            }
            catch (Exception ex)
            {
                StopTimer();
                UnBlockUi();
                StopIndicator();
                await DisplayAlert("Something went wrong", ex.Message, "Ok");
            }
        }
        private void BlockUi()
        {
            btnSelect.IsEnabled = false;
            TxtFileName.IsVisible = true;
            BtnCancel.IsVisible = true;
            BtnCancel.IsEnabled = false;
            BtnMake.IsVisible = true;
            //BtnViewAll.IsVisible = true;
            TxtFileName.Text = Settings.DefaultFileName;
        }
        private void UnBlockUi()
        {
            Files = Enumerable.Empty<FileResult>();
            btnSelect.IsEnabled = true;
            TxtFileName.IsVisible = false;
            BtnCancel.IsVisible = false;
            BtnMake.IsVisible = false;
            //BtnViewAll.IsVisible = false;
            BtnCancel.Text = "Cancel";
        }
        private void StartTimer()
        {
            LblTime.IsVisible = true;
            stopwatch.Start();
        }
        private void StopTimer()
        {
            LblTime.IsVisible = false;
            stopwatch.Stop();
            stopwatch.Reset();
            LblTime.Text = "";
        }
        private void TimerChanged(object sender)
        {
            if (stopwatch.IsRunning)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LblTime.Text = "Time Elapsed: " + stopwatch.Elapsed.ToString("hh\\:mm\\:ss");
                });
            }
        }
        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Are you sure?", "You want to cancel?", "Yes", "No"))
            {
                BtnCancel.IsEnabled = true;
                BtnCancel.Text = "Cancelling";
                CancelTask();

            }
        }
        private void RunIndicator()
        {
            ActivityInd.IsVisible = true;
            ActivityInd.IsRunning = true;
            BtnMake.IsEnabled = false;
        }
        private void CancelTask()
        {
            cancellationToken.Register(() =>
            {
                UnBlockUi();
                StopTimer();
                StopIndicator();
                Common.Instance.MakeToast("Task is cancelled", ToastLength.Long);
            });
            if (cancellationToken.CanBeCanceled)
            {
                src.Cancel();
            }
        }
        private void StopIndicator()
        {
            ActivityInd.IsVisible = false;
            ActivityInd.IsRunning = false;
            BtnMake.IsEnabled = true;
        }

        private async void BtnViewAll_Clicked(object sender, EventArgs e)
        {
            var files = Files;
            var page = new ViewImagesPage(ref files);
            await Navigation.PushAsync(page);
            Files = files.OrderBy(s => s.FileName);
        }
    }
}
