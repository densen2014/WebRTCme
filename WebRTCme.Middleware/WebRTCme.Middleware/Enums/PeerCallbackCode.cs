﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WebRTCme;

namespace WebRTCme.Middleware
{
    [JsonConverter(typeof(JsonCamelCaseStringEnumConverter))]
    public enum PeerCallbackCode
    {
        PeerJoined,
        PeerModified,
        PeerLeft
    }
}
