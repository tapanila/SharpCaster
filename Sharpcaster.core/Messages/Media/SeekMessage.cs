﻿using System.Runtime.Serialization;

namespace Sharpcaster.Core.Messages.Media
{
    /// <summary>
    /// Seek message
    /// </summary>
    [DataContract]
    class SeekMessage : MediaSessionMessage
    {
        /// <summary>
        /// Gets or sets the current time
        /// </summary>
        [DataMember(Name = "currentTime")]
        public double CurrentTime { get; set; }
    }
}
