﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using TranslateApp.Models;

namespace TranslateApp
{
    public partial class Image2Text : ContentPage
    {
        public Image2Text()
        {
            InitializeComponent();
        }


        async Task GettextfromImage(MediaFile file)
        {
            loading.IsRunning = true;

            ResultLabel.Text = "";
            var client = new HttpClient();


            const string uriBase = "https://westus.api.cognitive.microsoft.com/vision/v1.0/recognizeText";

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7c5eb61d251545a685d5d7bbe47d90bd");
            string requestParameters;

            if (Handwritten.IsToggled == true)
            {
                 requestParameters = "handwriting=true";
            }
            else {

                requestParameters = "handwriting=false";
            }
           

            // Assemble the URI for the REST API Call.
            string url = uriBase + "?" + requestParameters;



            HttpResponseMessage response;
            string operationLocation;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                    string contentString;
                    int i = 0;
                    do
                    {
                        await Task.Delay(5000);
                        response = await client.GetAsync(operationLocation);
                        contentString = await response.Content.ReadAsStringAsync();
                        ++i;
                    }
                    while (i < 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1);

                    if (i == 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1)
                    {
                        ResultLabel.Text += ("\nTimeout error.\n");
                        return;
                    }



                    JObject rss = JObject.Parse(contentString);



                    VisualtextJson.RootObject rootObject;
                    rootObject = JsonConvert.DeserializeObject<VisualtextJson.RootObject>(contentString);

                    foreach (var lines in rootObject.recognitionResult.lines)
                    {

                        foreach (Char text in lines.text)
                        {
                            ResultLabel.Text += text;

                        }

                    }

                }
                else
                {

                    ResultLabel.Text += "API error";
                }

                ResultLabel.IsVisible = true;
                loading.IsRunning = false;
                //Get rid of file once we have finished using it
                file.Dispose();
            }
        }

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("error", "Camera unavailable", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;



      
            await GettextfromImage(file);
        }



        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

       
       
    }
}