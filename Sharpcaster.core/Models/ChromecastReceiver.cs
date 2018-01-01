﻿using System;
using System.Collections.Generic;

namespace Sharpcaster.Core.Models
{
    /// <summary>
    /// Describesc chromecast receiver
    /// </summary>
    public class ChromecastReceiver
    {
        /// <summary>
        /// URI where device can be found
        /// </summary>
        public Uri DeviceUri { get; set; }
        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The port where device wants to receive messages
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Model information of device (Chromecast, Chromecast Audio and etc)
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Version information
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Status information
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// The device might send more interesting data when responding to DNS query. Raw data is stored here
        /// </summary>
        public IDictionary<string,string> ExtraInformation { get; set; }
    }
}