﻿using DemoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebRTCme.Middleware;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarinme;

namespace DemoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("TurnServerNamesJson", "TurnServerNamesJson")]
    public partial class ConnectionParametersPage : ContentPage
    {
        private readonly ConnectionParametersViewModel _connectionParametersViewModel;

        public ConnectionParametersPage()
        {
            InitializeComponent();
            _connectionParametersViewModel = App.Host.Services.GetService<ConnectionParametersViewModel>();
            BindingContext = _connectionParametersViewModel;
        }

        public string TurnServerNamesJson
        {
            set
            {
                var turnServerNamesJson = Uri.UnescapeDataString(value);
                var turnServerNames = JsonSerializer.Deserialize<string[]>(turnServerNamesJson).ToList();
                _connectionParametersViewModel.TurnServerNames = turnServerNames;
            }
        }
    }
}