﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WebRTCme.Middleware;
using System.Linq;

namespace WebRTCme.Middleware.Services
{
    public class MediaManagerService : IMediaManagerService
    {
        // Will be used as 'ItemsSource'. 
        public ObservableCollection<MediaParameters> MediaParametersList { get; set; } = new();
        //private Dictionary<string/*PeerUserName*/, MediaParameters> _peers = new();


        public void Add(MediaParameters mediaParameters)
        {
            MediaParametersList.Add(mediaParameters);
        }

        public void Remove(string label)
        {
            MediaParametersList.Remove(MediaParametersList.Single(mp => mp.Label == label));
        }

        public void Update(MediaParameters mediaParameters)
        {
            var current = MediaParametersList.Single(mp => mp.Label == mediaParameters.Label);
            current = mediaParameters;
        }
    }
}
