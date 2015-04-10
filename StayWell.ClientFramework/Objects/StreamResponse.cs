using System;
using System.IO;
using System.Net;
using System.Web;

namespace Staywell.ClientFramework.Objects
{
	public class StreamResponse : Stream
	{
		private const int DEFAULT_CONTENT_LENGTH = -1;
		private readonly WebResponse _webResponse;
		private readonly HttpResponse _httpResponse;
		private readonly Stream _stream;
		private string _contentType;
		private long _bytesWritten;

		public StreamResponse(HttpResponse response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			_httpResponse = response;
			_stream = response.OutputStream;

			ContentLength = DEFAULT_CONTENT_LENGTH;
		}

		public StreamResponse(WebResponse response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			_webResponse = response;
			_stream = response.GetResponseStream();

			ContentLength = DEFAULT_CONTENT_LENGTH;
		}

		public StreamResponse(Stream stream, string contentType = null)
		{
			_stream = stream;

			ContentLength = DEFAULT_CONTENT_LENGTH;

			ContentType = contentType;
		}

		public int ContentLength { get; set; }

		public string ContentType
		{
			get
			{
				if (_webResponse != null)
					return _webResponse.ContentType;
				if (_httpResponse != null)
					return _httpResponse.ContentType;
				return _contentType;
			}
			set
			{
				if (_webResponse != null)
					_webResponse.ContentType = value;
				else if (_httpResponse != null)
					_httpResponse.ContentType = value;
				else
					_contentType = value;
			}
		}

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
			_bytesWritten += count;
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

		public bool HasData
		{
			get { return _bytesWritten != 0; }
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

		public override long Position { get { return _stream.Position; } set { _stream.Position = value; } }

		#endregion
	}
}
