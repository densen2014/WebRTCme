﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRtcJsInterop.Extensions;
using WebRtcJsInterop.Interops;
using WebRTCme;

namespace WebRtcJsInterop.Api
{
    internal class MediaStream : ApiBase, IMediaStream
    {
        public bool Active => throw new NotImplementedException();

        ////public Task<bool> Ended => throw new NotImplementedException();

        public string Id => throw new NotImplementedException();

        private MediaStream(IJSRuntime jsRuntime, JsObjectRef jsObjectRef) : base(jsRuntime, jsObjectRef) { }


        public List<IMediaStreamTrack> GetTracks()
        {
            var jsObjectRef = JsRuntime.CallJsMethod<JsObjectRef>(SelfNativeObject, "getTracks");
            var jsObjectRefMediaStreamTrackArray = JsRuntime.GetJsPropertyArray(jsObjectRef);
            var mediaStreamTracks = new List<IMediaStreamTrack>();
            foreach(var jsObjectRefMediaStreamTrack in jsObjectRefMediaStreamTrackArray)
            {
                mediaStreamTracks.Add(MediaStreamTrack.Create(JsRuntime, jsObjectRefMediaStreamTrack));
            }
            return mediaStreamTracks;
        }


        internal static IMediaStream Create(IJSRuntime jsRuntime)
        {
            var jsObjectRef = jsRuntime.CreateJsObject("window", "MediaStream");
            var mediaStream = new MediaStream(jsRuntime, jsObjectRef);
            return mediaStream;
        }
        internal static IMediaStream Create(IJSRuntime jsRuntime, JsObjectRef jsObjectRefMediaStream)
        {
            var mediaStream = new MediaStream(jsRuntime, jsObjectRefMediaStream);
            return mediaStream;
        }

        public IMediaStream Clone()
        {
            throw new NotImplementedException();
        }

        public IMediaStreamTrack GetTrackById(string id)
        {
            throw new NotImplementedException();
        }

        public List<IMediaStreamTrack> GetVideoTracks()
        {
            throw new NotImplementedException();
        }

        public List<IMediaStreamTrack> GetAudioTracks()
        {
            throw new NotImplementedException();
        }

        public void AddTrack(IMediaStreamTrack track)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrack(IMediaStreamTrack track)
        {
            throw new NotImplementedException();
        }

        public void SetElementReferenceSrcObject(object/*ElementReference*/ elementReference)
        {
            JsRuntime.SetJsProperty(elementReference, "srcObject", SelfNativeObject);

            //await JsRuntime.InvokeVoidAsync(
            //    "objectRef.set",
            //    new object[]
            //    {
            //        elementReference,
            //        "srcObject",
            //        JsObjectRef
            //    });

        }

    }
}
