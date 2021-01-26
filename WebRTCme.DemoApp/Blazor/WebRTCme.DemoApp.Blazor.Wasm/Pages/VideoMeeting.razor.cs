﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebRTCme;
using WebRTCme.Middleware;
using WebRTCme.Middleware.Blazor;

//// TODO: TESTING FOR NOW, MOVE ALL WEBRTC CODE to Middleware
namespace WebRTCme.DemoApp.Blazor.Wasm.Pages
{
    partial class VideoMeeting : IDisposable
    {
        [Inject]
        IJSRuntime JsRuntime { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }


        VideoType LocalType { get; set; } = VideoType.Camera;

        string LocalSource { get; set; } = "Default";
        
        IMediaStream LocalStream { get; set; }

        VideoType Remote1Type { get; set; } = VideoType.Room;

        string Remote1Source { get; set; }

        IMediaStream Remote1Stream { get; set; }



        private IWebRtcMiddleware _webRtcMiddleware;
        private IRoomService _roomService;
        private IMediaStreamService _mediaStreamService;
        private string[] _turnServerNames;

        private JoinRoomRequestParameters JoinRoomRequestParameters { get; set; } = new()
        //// Useful during development. DELETE THIS LATER!!!
   { TurnServerName="StunOnly", RoomName="hello", UserName="melih"}
            ;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _webRtcMiddleware = CrossWebRtcMiddleware.Current;
            _mediaStreamService = await _webRtcMiddleware.CreateMediaStreamServiceAsync(JsRuntime);
            LocalStream = await _mediaStreamService.GetCameraStreamAsync(LocalSource);

            _roomService = await _webRtcMiddleware.CreateRoomServiceAsync(Configuration["SignallingServer:BaseUrl"], 
                JsRuntime);
            _turnServerNames = await _roomService.GetTurnServerNames();
            if (_turnServerNames is not null)
                JoinRoomRequestParameters.TurnServerName = _turnServerNames[0];
        }

        private void HandleValidSubmit()
        {
            JoinRoomRequestParameters.LocalStream = LocalStream;
            var peerCallbackDisposer = _roomService.JoinRoomRequest(JoinRoomRequestParameters).Subscribe(
                onNext: (peerCallbackParameters) => 
                { 
                    switch (peerCallbackParameters.Code)
                    {
                        case PeerCallbackCode.PeerJoined:
                            Remote1Stream = peerCallbackParameters.MediaStream;
                            Remote1Source = peerCallbackParameters.PeerUserName;
                            StateHasChanged();
                            break;

                        case PeerCallbackCode.PeerModified:
                            break;

                        default:
                            break;
                    }
                },
                onError: (exception) => 
                { 
                },
                onCompleted: () => 
                { 
                });
        }

        public void Dispose()
        {
            //// TODO: How to call async in Dispose??? Currently fire and forget!!!
            Task.Run(async () => await _roomService.DisposeAsync());
            _webRtcMiddleware.Dispose();
        }
    }
}

