﻿namespace Renci.SshClient.Common
{
    /// <summary>
    /// Provides data for <see cref="Renci.SshClient.Channels.Channel.OpenFailed"/> event.
    /// </summary>
    internal class ChannelOpenFailedEventArgs : ChannelEventArgs
    {
        /// <summary>
        /// Gets failure reason code.
        /// </summary>
        public uint ReasonCode { get; private set; }

        /// <summary>
        /// Gets failure description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets failure language.
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelOpenFailedEventArgs"/> class.
        /// </summary>
        /// <param name="channelNumber">Channel number.</param>
        /// <param name="reasonCode">Failure reason code.</param>
        /// <param name="description">Failure description.</param>
        /// <param name="language">Failure language.</param>
        public ChannelOpenFailedEventArgs(uint channelNumber, uint reasonCode, string description, string language)
            : base(channelNumber)
        {
            this.ReasonCode = reasonCode;
            this.Description = description;
            this.Language = language;
        }
    }
}
