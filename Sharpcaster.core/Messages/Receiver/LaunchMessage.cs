﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sharpcaster.Core.Messages.Receiver
{
    /// <summary>
    /// Launch message
    /// </summary>
    [DataContract]
    class LaunchMessage : MessageWithId
    {
        /// <summary>
        /// Gets or sets the application identifier
        /// </summary>
        [DataMember(Name = "appId")]
        public string ApplicationId { get; set; }
    }
}
