﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebRTCme.MediaSoupClient.Models
{
    class H264Parameters
    {
        [JsonPropertyName("packetization-mode")]
        public int? PacketizationMode { get; init; }

        [JsonPropertyName("profile-level-id")]
        public string ProfileLevelId { get; set; }

        [JsonPropertyName("level-asymmetry-allowed")]
        public int? LevelAsymmetryAllowed { get; set; }

        [JsonPropertyName("x-google-start-bitrate")]
        public int? XGoogleStartBitrate { get; init; }

        [JsonPropertyName("x-google-max-bitrate")]
        public int? XGoogleMaxBitrate { get; init; }

        [JsonPropertyName("x-google-min-bitrate")]
        public int? XGoogleMinBitrate { get; init; }

//'{"packetization-mode":1,"level-asymmetry-allowed":1,"profile-level-id":"4d0032","x-google-start-bitrate":1000}'
    }
}
