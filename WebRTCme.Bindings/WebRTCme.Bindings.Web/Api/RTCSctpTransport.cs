﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebRtcBindingsWeb.Interops;
using WebRtcBindingsWeb.Extensions;
using WebRTCme;

namespace WebRtcBindingsWeb.Api
{
    internal class RTCSctpTransport : ApiBase, IRTCSctpTransport
    {
        internal static IRTCSctpTransport Create(IJSRuntime jsRuntime, JsObjectRef jsObjectRefSctpTransport)
        {
            return new RTCSctpTransport(jsRuntime, jsObjectRefSctpTransport);
        }

        private RTCSctpTransport(IJSRuntime jsRuntime, JsObjectRef jsObjectRef) : base(jsRuntime, jsObjectRef) 
        {
            AddNativeEventListener("onstatechange", OnStateChange);
        }

        public int MaxChannels => GetNativeProperty<int>("maxChannels");

        public int MaxMessageSize => GetNativeProperty<int>("maxMessageSize");

        public RTCSctpTransportState State => GetNativeProperty<RTCSctpTransportState>("state");

        public IRTCSctpTransport Transport =>
            RTCSctpTransport.Create(JsRuntime, JsRuntime.GetJsPropertyObjectRef(NativeObject, "transport"));

        public event EventHandler<RTCSctpTransportState> OnStateChange;
    }
}
