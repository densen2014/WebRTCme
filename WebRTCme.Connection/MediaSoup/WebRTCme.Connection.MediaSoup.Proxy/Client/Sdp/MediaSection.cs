﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Utilme.SdpTransform;
using WebRTCme.Connection.MediaSoup.Proxy.Models;

namespace WebRTCme.Connection.MediaSoup.Proxy.Client.Sdp
{
    abstract class MediaSection
    {
        protected readonly MediaObject _mediaObject;
        protected readonly bool _planB;

        readonly IceParameters _iceParameters;
        readonly IceCandidate[] _iceCandidates;
        readonly DtlsParameters _dtlsParameters;

        protected MediaSection(IceParameters iceParameters, IceCandidate[] iceCandidates, 
            DtlsParameters dtlsParameters, bool planB)
        {
            _iceParameters = iceParameters;
            _iceCandidates = iceCandidates;
            _dtlsParameters = dtlsParameters;
            _planB = planB;

            _mediaObject = new() 
            { 
                BinaryAttributes = new()
            };

            if (iceParameters is not null)
            {
                SetIceParameters(iceParameters);
            }
            
            if (iceCandidates is not null)
            {
                _mediaObject.Candidates = new();
                foreach (var iceCandidate in iceCandidates)
                {
                    Candidate candidate = new()
                    {
                        Foundation = iceCandidate.Foundation,
                        ComponentId = 1,    // RTP
                        Transport = iceCandidate.Protocol.ToSdp(), 
                        Priority = iceCandidate.Priority,
                        ConnectionAddress = iceCandidate.Ip,
                        Port = iceCandidate.Port,
                        Type = iceCandidate.Type.ToSdp()
                    };
                    _mediaObject.Candidates.Add(candidate);
                }
            }

            _mediaObject.IceOptions = new IceOptions { Tags = new string[] { "renomination" } };

            if (dtlsParameters is not null)
            {
                SetDtlsRole(dtlsParameters.DtlsRole);
            }
        }

        public MediaObject MediaObject => _mediaObject;
        public Mid Mid => _mediaObject.Mid;
        public bool Closed => _mediaObject.Port == 0;

        protected abstract void SetDtlsRole(DtlsRole? dtlsRole);

        public void SetIceParameters(IceParameters iceParameters)
        {
            _mediaObject.IceUfrag = new IceUfrag { Ufrag = iceParameters.UsernameFragment };
            _mediaObject.IcePwd = new IcePwd { Password = iceParameters.Password };
        }



        public void Disable()
        {
            _mediaObject.Direction = Direction.Inactive;

            _mediaObject.Extensions = null;
            _mediaObject.Ssrcs = null;
            _mediaObject.SsrcGroups = null;
            _mediaObject.Simulcast = null;
            _mediaObject.Simulcast03 = null;
            _mediaObject.Rids = null;
        }

        public void Close()
        {
            _mediaObject.Direction = Direction.Inactive;
            _mediaObject.Port = 0;

            _mediaObject.Extensions = null;
            _mediaObject.Ssrcs = null;
            _mediaObject.SsrcGroups = null;
            _mediaObject.Simulcast = null;
            _mediaObject.Simulcast03 = null;
            _mediaObject.Rids = null;
            _mediaObject.ExtmapAllowMixed = null;
        }

        protected string GetCodecName(RtpCodecParameters codec)
        {
	        var mimeTypeRegex = new Regex("'^(audio|video)/(.+)");
            var mimeTypeMatch = mimeTypeRegex.Match(codec.MimeType);

	        if (!mimeTypeMatch.Success)
		        throw new Exception("'invalid codec.mimeType");

	        return mimeTypeMatch.Value;
        }


        protected string CodecParametersToFmtpValue(CodecParameters codecParameters)
        {
            List<string> values = new();
            
            if (codecParameters.Stereo.HasValue)
                values.Add($"stereo={((bool)codecParameters.Stereo ? 1 : 0)}");
            if (codecParameters.UseInBandFec.HasValue)
                values.Add($"useinbandfec{((bool)codecParameters.UseInBandFec ? 1 : 0)}");
            if (codecParameters.UsedTx.HasValue)
                values.Add($"usedtx={((bool)codecParameters.UsedTx ? 1 : 0)}");
            if (codecParameters.MaxAverageBitrate.HasValue)
                values.Add($"maxplaybackrate={(int)codecParameters.MaxPlaybackRate}");
            if (codecParameters.MaxAverageBitrate.HasValue)
                values.Add($"maxaveragebitrate={(int)codecParameters.MaxAverageBitrate}");
            if (codecParameters.Ptime.HasValue)
                values.Add($"ptime={(int)codecParameters.Ptime}");
            if (codecParameters.XGoogleStartBitrate.HasValue)
                values.Add($"x-google-start-bitrate={(int)codecParameters.XGoogleStartBitrate}");
            if (codecParameters.XGoogleMaxBitrate.HasValue)
                values.Add($"x-google-max-bitrate={(int)codecParameters.XGoogleMaxBitrate}");
            if (codecParameters.XGoogleMinBitrate.HasValue)
                values.Add($"x-google-min-bitrate={(int)codecParameters.XGoogleMinBitrate}");

            return string.Join(";", values.ToArray());
        }
    }
}
