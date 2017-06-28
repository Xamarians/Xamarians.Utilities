using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarians.Media;
using Xamarians.Utilities.Interface;
using Xamarin.Forms;

namespace Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            DependencyService.Get<IMobileServices>().OnStatusChanged += MainPage_OnStatusChanged;
        }

        private void MainPage_OnStatusChanged(object sender, EventArgs e)
        {
            if (DependencyService.Get<IMobileServices>().CheckInternetConnection())
                DisplayAlert("Alert", "Internet is connected", "Ok");
            else
                DisplayAlert("Alert", "Internet is not connected", "Ok");
        }

        private void OnSmsButtonClicked(object sender, EventArgs e)
        {
            string[] number = new string[]
            {
                "9999999999",
                "1111111111",
                "2222222222"
            };
            string message = "Hi Ronit, how are you?";
            DependencyService.Get<IMobileServices>().SendSms(number, message);
        }
        private void OnCallButtonClicked(object sender, EventArgs e)
        {
            var isInternetConnected = DependencyService.Get<IMobileServices>().CheckInternetConnection();
            if (isInternetConnected)
                DependencyService.Get<IMobileServices>().DoCall("9999999999");
            else
            {
                DisplayAlert("error", "Internet is disconnected", "ok");
            }
        }

        private void OnEmailButtonClicked(object sender, EventArgs e)
        {
            string message = "Hi Ronit, how are you?";
            string[] reciever = new string[] 
            {
                "person@gmail.com",
                "person1@gmail.com"
            };
            string[] cc = new string[]
            {
                "person2@gmail.com",
                "person3@gmail.com"
            };
            string[] bcc = new string[]
            {
                "person4@gmail.com",
                "person5@gmail.com"
            };
            string subject = "Test";
            DependencyService.Get<IMobileServices>().SendEmail(message, reciever, cc, bcc, subject);
        }

        private string GenerateFilePath()
        {
            return Path.Combine(MediaService.Instance.GetPublicDirectoryPath(), MediaService.Instance.GenerateUniqueFileName("jpg"));
        }

     
        private async void OnCameraClicked(object sender, System.EventArgs e)
        {
            string filePath = GenerateFilePath();
            var result = await MediaService.Instance.TakePhotoAsync(new CameraOption()
            {
                FilePath = filePath,
                MaxWidth = 300,
                MaxHeight = 300
            });
            if (result.IsSuccess)
            {
                image.Source = result.FilePath;
            }

        }

        private async void OnPhotoGalleryClicked(object sender, System.EventArgs e)
        {
            var filePath = await MediaService.Instance.OpenMediaPickerAsync(MediaType.Image);
            image.Source = filePath.FilePath;
        }
        private async void OnVideoGalleryClicked(object sender, System.EventArgs e)
        {
            var result = await MediaService.Instance.OpenMediaPickerAsync(MediaType.Video);
            if (result.IsSuccess)
                await DisplayAlert("Success", result.FilePath, "OK");
            else
                await DisplayAlert("Error", result.Message, "OK");
        }
        private async void OnResizeImageClicked(object sender, System.EventArgs e)
        {
            var inputPath = await MediaService.Instance.OpenMediaPickerAsync(MediaType.Image);
            string resizeFilePath = GenerateFilePath();
            var filePath = await MediaService.Instance.ResizeImageAsync(inputPath.FilePath, resizeFilePath, 250, 250);
            image.Source = resizeFilePath;
        }


        private async void OnAudioPickerClicked(object sender, System.EventArgs e)
        {
            var result = await MediaService.Instance.OpenMediaPickerAsync(MediaType.Audio);
            if (result.IsSuccess)
                await DisplayAlert("Success", result.FilePath, "OK");
            else
                await DisplayAlert("Error", result.Message, "OK");

        }
    }
}

    

