namespace FolkerKinzel.VCards.Tests;

internal class StreamDummy : Stream
{
    private readonly Stream _stream;

    public StreamDummy(Stream stream, bool canSeek = true, bool canWrite = true, bool canRead = true)
    {
        _stream = stream;
        CanSeek = canSeek;
        CanWrite = canWrite;
        CanRead = canRead;
    }

    public override bool CanRead { get; }

    public override bool CanSeek { get; }

    public override bool CanWrite {  get; }

    public override long Length => _stream.Length;

    public override long Position
    { 
        get
        {
            if (!CanSeek)
            {
                throw new NotSupportedException();
            }

            return _stream.Position;
        }
        set
        {
            if (!CanSeek)
            {
                throw new NotSupportedException();
            }

            _stream.Position = value;
        }
    }

    public override void Flush() => _stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
        => !CanRead ? throw new NotSupportedException() : _stream.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin)
        => !CanSeek ? throw new NotSupportedException()
                    : _stream.Seek(offset, origin);

    public override void SetLength(long value) =>  _stream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) { }
}
