﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebRTCme.MediaSoupClient.Models
{
    class TransportInfo
    {
        public string Id { get; init; }
        public IceParameters IceParameters { get; init; }

        public IceCandidate[] IceCandidates { get; init; }

        public DtlsParameters DtlsParameters { get; init; }

        public SctpParameters SctpParameters { get; init; }

    }
}
