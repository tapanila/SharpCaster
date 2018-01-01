﻿using Sharpcaster.Core.Interfaces;
using Sharpcaster.Core.Messages.Heartbeat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Sharpcaster.Core.Channels
{
    /// <summary>
    /// Heartbeat channel. Responds to ping messages with pong message
    /// </summary>
    public class HeartbeatChannel : ChromecastChannel, IHeartbeatChannel
    {
        /// <summary>
        /// Initializes a new instance of HeartbeatChannel class
        /// </summary>
        public HeartbeatChannel() : base("tp.heartbeat")
        {
        }

        /// <summary>
        /// Called when a message for this channel is received
        /// </summary>
        /// <param name="message">message to process</param>
        public override async Task OnMessageReceivedAsync(IMessage message)
        {
            if (message is PingMessage)
            {
                await SendAsync(new PongMessage());
            }
            else
            {
                //TODO: Remove this if we don't hit this
                Debugger.Break();
            }
        }
    }
}
