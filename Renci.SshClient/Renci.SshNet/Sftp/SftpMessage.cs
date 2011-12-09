﻿using System;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet.Common;
using System.Globalization;
using Renci.SshNet.Sftp.Responses;

namespace Renci.SshNet.Sftp
{
    internal abstract class SftpMessage : SshData
    {
        public new static SftpMessage Load(byte[] data)
        {
            var messageType = (SftpMessageTypes)data.FirstOrDefault();

            return Load(data, messageType);
        }

        protected override int ZeroReaderIndex
        {
            get
            {
                return 1;
            }
        }

        public abstract SftpMessageTypes SftpMessageType { get; }

        protected override void LoadData()
        {
        }

        protected override void SaveData()
        {
            this.Write((byte)this.SftpMessageType);
        }

        protected SftpFileAttributes ReadAttributes()
        {

            var flag = this.ReadUInt32();

            long size = -1;
            int userId = -1;
            int groupId = -1;
            uint permissions = 0;
            var accessTime = DateTime.MinValue;
            var modifyTime = DateTime.MinValue;
            IDictionary<string, string> extensions = null;

            if ((flag & 0x00000001) == 0x00000001)   //  SSH_FILEXFER_ATTR_SIZE
            {
                size = (long)this.ReadUInt64();
            }

            if ((flag & 0x00000002) == 0x00000002)   //  SSH_FILEXFER_ATTR_UIDGID
            {
                userId = (int)this.ReadUInt32();

                groupId = (int)this.ReadUInt32();
            }

            if ((flag & 0x00000004) == 0x00000004)   //  SSH_FILEXFER_ATTR_PERMISSIONS
            {
                permissions = this.ReadUInt32();
            }

            if ((flag & 0x00000008) == 0x00000008)   //  SSH_FILEXFER_ATTR_ACMODTIME
            {
                var time = this.ReadUInt32();
                accessTime = DateTime.FromFileTime((time + 11644473600) * 10000000);
                time = this.ReadUInt32();
                modifyTime = DateTime.FromFileTime((time + 11644473600) * 10000000);
            }

            if ((flag & 0x80000000) == 0x80000000)   //  SSH_FILEXFER_ATTR_ACMODTIME
            {
                var extendedCount = this.ReadUInt32();
                extensions = this.ReadExtensionPair();
            }
            var attributes = new SftpFileAttributes(accessTime, modifyTime, size, userId, groupId, permissions, extensions);

            return attributes;
        }

        protected void Write(SftpFileAttributes attributes)
        {
            if (attributes == null)
            {
                this.Write((uint)0);
                return;
            }
            else
            {
                UInt32 flag = 0;

                if (attributes.Size > -1)
                {
                    flag |= 0x00000001;
                }

                if (attributes.UserId > -1 && attributes.GroupId > -1)
                {
                    flag |= 0x00000002;
                }

                if (attributes.Permissions > 0)
                {
                    flag |= 0x00000004;
                }

                if (attributes.LastAccessTime > DateTime.MinValue && attributes.LastWriteTime > DateTime.MinValue)
                {
                    flag |= 0x00000008;
                }

                if (attributes.Extensions != null)
                {
                    flag |= 0x80000000;
                }

                this.Write(flag);

                if (attributes.Size > -1)
                {
                    this.Write((UInt64)attributes.Size);
                }

                if (attributes.UserId > -1 && attributes.GroupId > -1)
                {
                    this.Write((UInt32)attributes.UserId);
                    this.Write((UInt32)attributes.GroupId);
                }

                if (attributes.Permissions > 0)
                {
                    this.Write(attributes.Permissions);
                }

                if (attributes.LastAccessTime > DateTime.MinValue && attributes.LastWriteTime > DateTime.MinValue)
                {
                    uint time = (uint)(attributes.LastAccessTime.ToFileTime() / 10000000 - 11644473600);
                    this.Write(time);
                    time = (uint)(attributes.LastWriteTime.ToFileTime() / 10000000 - 11644473600);
                    this.Write(time);
                }

                if (attributes.Extensions != null)
                {
                    this.Write(attributes.Extensions);
                }
            }
        }

        private static SftpMessage Load(byte[] data, SftpMessageTypes messageType)
        {
            SftpMessage message = null;

            switch (messageType)
            {
                case SftpMessageTypes.Version:
                    message = new SftpVersionResponse();
                    break;
                case SftpMessageTypes.Status:
                    message = new SftpStatusResponse();
                    break;
                case SftpMessageTypes.Data:
                    message = new SftpDataResponse();
                    break;
                case SftpMessageTypes.Handle:
                    message = new SftpHandleResponse();
                    break;
                case SftpMessageTypes.Name:
                    message = new SftpNameResponse();
                    break;
                case SftpMessageTypes.Attrs:
                    message = new SftpAttrsResponse();
                    break;
                case SftpMessageTypes.ExtendedReply:
                    message = new SftpExtendedReplyResponse();
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Message type '{0}' is not supported.", messageType));
            }

            message.LoadBytes(data);

            message.ResetReader();

            message.LoadData();

            return message;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "SFTP Message : {0}", this.SftpMessageType);
        }
    }
}
