using System.IO;

namespace StayWell.Interface
{
	public class StreamRequest : Stream
	{
		// -1 is the default for http request streams
		private const int DEFAULT_CONTENT_LENGTH = -1;
		private readonly Stream _stream;

		public StreamRequest(Stream stream)
		{
			_stream = stream;

			ContentLength = DEFAULT_CONTENT_LENGTH;
		}

		public string ContentType { get; set; }
		public int ContentLength { get; set; }

		#region Overrides of Stream

		public override void Flush()
		{
			_stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _stream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
		}

		public override bool CanRead
		{
			get { return _stream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return _stream.CanSeek; }
		}

		public override bool CanWrite
		{	
			get { return _stream.CanWrite; }
		}

		public override long Length
		{
			get { return _stream.Length; }
		}

		public override void Close()
		{
			_stream.Close();
		}

		public override bool CanTimeout
		{
			get { return _stream.CanTimeout; }
		}

		public override long Position { get { return _stream.Position; } set { _stream.Position = value; }}

		#endregion
	}
}
