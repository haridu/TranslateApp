using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TranslateApp
{
    public partial class AzureTable : ContentPage
    {
		Geocoder geoCoder;
       
        public AzureTable()
        {
            InitializeComponent();
            geoCoder = new Geocoder();

		}

		async void Handle_ClickedAsync(object sender, System.EventArgs e)
		{
            loading.IsRunning = true;
			List<Translate2Model> Translate2Information = await AzureManager.AzureManagerInstance.GetTranslate2Information();

			foreach (Translate2Model model in Translate2Information)
			{
				var position = new Position(model.Latitude, model.Longitude);
				var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                //foreach (var address in possibleAddresses)
                //notHotDog  model.City = address;
            }

            HotDogList.ItemsSource = Translate2Information;
            loading.IsRunning = false;
		}

    }
}
