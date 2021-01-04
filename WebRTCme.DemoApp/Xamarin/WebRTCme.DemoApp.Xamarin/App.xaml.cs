﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DemoApp.Services;
using DemoApp.Views;
using WebRTCme.Middleware.Xamarin;

namespace DemoApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // TODO: Move URL to a config file.
            WebRtcMiddleware.Initialize("https://localhost:5051");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void CleanUp()
        {
            WebRtcMiddleware.Cleanup();
            base.CleanUp();
        }
    }
}
