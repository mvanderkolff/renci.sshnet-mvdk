﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Renci.SshNet.Compression
{
    public class ZlibStream
    {
        //private readonly Ionic.Zlib.ZlibStream _baseStream;

        public ZlibStream(Stream stream, CompressionMode mode)
        {
            //switch (mode)
            //{
            //    case CompressionMode.Compress:
            //        this._baseStream = new Ionic.Zlib.ZlibStream(stream, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.Default);
            //        break;
            //    case CompressionMode.Decompress:
            //        this._baseStream = new Ionic.Zlib.ZlibStream(stream, Ionic.Zlib.CompressionMode.Decompress, Ionic.Zlib.CompressionLevel.Default);
            //        break;
            //    default:
            //        break;
            //}

            //this._baseStream.FlushMode = Ionic.Zlib.FlushType.Partial;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            //this._baseStream.Write(buffer, offset, count);
        }
    }
}
