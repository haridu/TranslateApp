﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TranslateApp
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();


        }


        async void Login_ClickedAsync(object sender, System.EventArgs e)
        {
            loading.IsRunning = true;
            List<Translate2Model> Translate2Information = await AzureLoginTableManager.AzureManagerInstance.GetTranslate2Information();

            foreach (Translate2Model model in Translate2Information)
            {
                if (username.Text == model.username)
                {

                    if (password.Text == model.password)
                    {

                        await Navigation.PushModalAsync(new Image2Text());


                    }
                    else
                    {
                        noticelabel.Text = "Wronge password";

                    }

                }

            }
            noticelabel.Text = "Wronge username";



            if (Translate2Information.Count < 1)
            {
                noticelabel.Text = "table is empty";
            }

            loading.IsRunning = false;
        }

    }
}
