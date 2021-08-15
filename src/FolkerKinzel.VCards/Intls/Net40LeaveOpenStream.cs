#if NET40
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls
{

    internal class Net40LeaveOpenStream : Stream
    {
        private readonly Stream _stream;

        internal Net40LeaveOpenStream(Stream stream) => this._stream = stream;

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

        public override void SetLength(long value) => _stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);

        public override void Close() { }

        protected override void Dispose(bool disposing) { }

        //public override ValueTask DisposeAsync() => base.DisposeAsync();

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) 
            => _stream.BeginRead(buffer, offset, count, callback, state);

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            => _stream.BeginWrite(buffer, offset, count, callback, state);

        public override bool CanTimeout => _stream.CanTimeout;

        public override ObjRef CreateObjRef(Type requestedType) => _stream.CreateObjRef(requestedType);

        //protected override WaitHandle CreateWaitHandle() => _stream.CreateWaitHandle();
        //protected override void ObjectInvariant() => _stream.ObjectInvariant();


        public override int EndRead(IAsyncResult asyncResult) => _stream.EndRead(asyncResult);

        public override void EndWrite(IAsyncResult asyncResult) => _stream.EndWrite(asyncResult);

        public override object InitializeLifetimeService() => _stream.InitializeLifetimeService();

        public override int ReadByte() => _stream.ReadByte();

        public override int ReadTimeout { get => _stream.ReadTimeout; set => _stream.ReadTimeout = value; }

        public override void WriteByte(byte value) => _stream.WriteByte(value);

        public override int WriteTimeout { get => _stream.WriteTimeout; set => _stream.WriteTimeout = value; }
    }
}
#endif
