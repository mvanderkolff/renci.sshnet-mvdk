﻿using System;
using System.Collections.Generic;

namespace Renci.SshClient.Messages.Authentication
{
    /// <summary>
    /// Represents SSH_MSG_USERAUTH_PK_OK message.
    /// </summary>
    [Message("SSH_MSG_USERAUTH_PK_OK", 60)]
    internal class PublicKeyMessage : Message
    {
        /// <summary>
        /// Gets the name of the public key algorithm.
        /// </summary>
        /// <value>
        /// The name of the public key algorithm.
        /// </value>
        public string PublicKeyAlgorithmName { get; private set; }

        /// <summary>
        /// Gets the public key data.
        /// </summary>
        public IEnumerable<byte> PublicKeyData { get; private set; }

        /// <summary>
        /// Called when type specific data need to be loaded.
        /// </summary>
        protected override void LoadData()
        {
            this.PublicKeyAlgorithmName = this.ReadString();
            this.PublicKeyData = this.ReadString().GetSshBytes();
        }

        /// <summary>
        /// Called when type specific data need to be saved.
        /// </summary>
        protected override void SaveData()
        {
            this.Write(this.PublicKeyAlgorithmName);
            this.Write(this.PublicKeyData.GetSshString());
        }
    }
}
