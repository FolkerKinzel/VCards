namespace FolkerKinzel.VCards.Tests;

internal class FailStream(Exception exception, Stream? innerStream = null, long throwPosition = 0) : Stream
{
    private readonly Stream? _innerStream = innerStream;
    private readonly Exception _exception = exception ?? throw new ArgumentNullException(nameof(exception));
    private readonly long _throwPosition = throwPosition;

    public override bool CanRead => _innerStream is null || _innerStream.CanRead;

    public override bool CanSeek => _innerStream is null || _innerStream.CanSeek;

    public override bool CanWrite => _innerStream is null || _innerStream.CanWrite;

    public override long Length => _innerStream is null ? 10000 : _innerStream.Length;

    public override long Position
    {
        get => _innerStream is null ? 0 : _innerStream.Position;

        set
        {
            if (_innerStream is not null)
            {
                _innerStream.Position = value;
            }
        }
    }

    public override void Flush() => _innerStream?.Flush();
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_innerStream is null) { throw _exception; }

        int bytesRead = _innerStream.Read(buffer, offset, count);
        return Position >= _throwPosition ? throw _exception : bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin) => _innerStream?.Seek(offset, origin) ?? Position;
    public override void SetLength(long value) => _innerStream?.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (_innerStream is null)
        {
            throw _exception;
        }
        Write(buffer, offset, count);
        if (Position >= _throwPosition)
        {
            throw _exception;
        }
    }
}
