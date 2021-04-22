using Android.Widget;
using PdfMaker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PdfMaker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MorePage : ContentPage
    {
        public MorePage()
        {
            InitializeComponent();
            ItemsListView.ItemsSource = new ObservableCollection<Item>(new List<Item>
            {
                new(){ Text="History",Description="View your pdf history"},
                new(){ Text="Settings",Description="Change application settings"},
                new(){ Text="Clear Cache",Description="Clears application's cache"}
            });
        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            StackLayout sendr = sender as StackLayout;
            var selected = sendr.Children[0] as Label;
            switch (selected.Text)
            {
                case "History":
                    await Navigation.PushAsync(new HistoryPage());
                    break;
                case "Settings":
                    await Navigation.PushAsync(new SettingsPage());
                    break;
                case "Clear Cache":
                    try
                    {
                        if (await DisplayAlert("Are you sure?", "You want to delete and clear all application's cache?\nYou have to face consequence.", "Yes", "No"))
                        {
                            await AppServices.ClearApplicationCache();
                            Toast.MakeText(Android.App.Application.Context, "Application cache cleared.", ToastLength.Short).Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Something went wrong", ex.Message, "Ok");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}