﻿using Sharpcaster.Core.Models.Media;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpcaster.Core.Interfaces
{
    /// <summary>
    /// Interface for the media channel
    /// </summary>
    public interface IMediaChannel : IStatusChannel<IEnumerable<MediaStatus>>, IChromecastChannel
    {
        /// <summary>
        /// Loads a media
        /// </summary>
        /// <param name="media">media to load</param>
        /// <param name="autoPlay">true to play the media directly, false otherwise</param>
        /// <returns>media status</returns>
        Task<MediaStatus> LoadAsync(Media media, bool autoPlay = true);

        /// <summary>
        /// Plays the media
        /// </summary>
        /// <returns>media status</returns>
        Task<MediaStatus> PlayAsync();

        /// <summary>
        /// Pauses the media
        /// </summary>
        /// <returns>media status</returns>
        Task<MediaStatus> PauseAsync();

        /// <summary>
        /// Stops the media
        /// </summary>
        /// <returns>media status</returns>
        Task<MediaStatus> StopAsync();

        /// <summary>
        /// Seeks to the specified time
        /// </summary>
        /// <param name="seconds">time in seconds</param>
        /// <returns>media status</returns>
        Task<MediaStatus> SeekAsync(double seconds);
    }
}
