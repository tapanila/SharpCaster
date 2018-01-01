﻿using Sharpcaster.Core.Interfaces;
using System;
using Sharpcaster.Core.Models.ChromecastStatus;
using System.Threading.Tasks;
using Sharpcaster.Core.Messages.Receiver;

namespace Sharpcaster.Core.Channels
{
    /// <summary>
    /// ReceiverChannel, Receives ChromecastStatus, volume, starting and stopping application
    /// </summary>
    public class ReceiverChannel : StatusChannel<ReceiverStatusMessage, ChromecastStatus>, IReceiverChannel
    {
        public ReceiverChannel() : base("receiver")
        {
        }

        public async Task<ChromecastStatus> GetChromecastStatusAsync()
        {
            return (await SendAsync<ReceiverStatusMessage>(new GetStatusMessage())).Status;
        }

        public async Task<ChromecastStatus> LaunchApplicationAsync(string applicationId)
        {
            return (await SendAsync<ReceiverStatusMessage>(new LaunchMessage() { ApplicationId = applicationId })).Status;
        }

        public async Task<ChromecastStatus> SetMute(bool muted)
        {
            return (await SendAsync<ReceiverStatusMessage>(new SetVolumeMessage() { Volume = new Models.Volume() { Muted = muted } })).Status;
        }

        public async Task<ChromecastStatus> SetVolume(double level)
        {
            if (level < 0 || level > 1.0)
            {
                throw new ArgumentException("level must be between 0.0 and 1.0", nameof(level));
            }
            return (await SendAsync<ReceiverStatusMessage>(new SetVolumeMessage() { Volume = new Models.Volume() { Level = level } })).Status;
        }

        public async Task<ChromecastStatus> StopApplication(string sessionId)
        {
            return (await SendAsync<ReceiverStatusMessage>(new StopMessage() { SessionId = sessionId })).Status;
        }
    }
}