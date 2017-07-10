using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpCaster.Channels;
using SharpCaster.Interfaces;
using System.Net.Sockets;
using System.Net.Security;

namespace SharpCaster.Services
{
    public class ChromecastSocketService : IChromecastSocketService
    {
        private static readonly object LockObject = new object();
        private TcpClient _client;
        private SslStream _stream;

        public async Task Initialize(string host, string port, ConnectionChannel connectionChannel, HeartbeatChannel heartbeatChannel, Action<Stream, bool, CancellationToken> packetReader, CancellationToken cancellationToken)
        {
            if (_client == null) _client = new TcpClient();
            await _client.ConnectAsync(host, int.Parse(port));

            var secureStream = new SslStream(_client.GetStream(), true, (sender, certificate, chain, sslPolicyErrors) => true);
            await secureStream.AuthenticateAsClientAsync(host);
            _stream = secureStream;


            await connectionChannel.OpenConnection();
            heartbeatChannel.StartHeartbeat();

            await Task.Run(async () =>
            {
                while (true)
                {
                    var sizeBuffer = new byte[4];
                    // First message should contain the size of message
                    await _stream.ReadAsync(sizeBuffer, 0, sizeBuffer.Length, cancellationToken);
                    // The message is little-endian (that is, little end first),
                    // reverse the byte array.
                    Array.Reverse(sizeBuffer);
                    //Retrieve the size of message
                    var messageSize = BitConverter.ToInt32(sizeBuffer, 0);
                    var messageBuffer = new byte[messageSize];
                    await _stream.ReadAsync(messageBuffer, 0, messageBuffer.Length, cancellationToken);
                    var answer = new MemoryStream(messageBuffer.Length);
                    await answer.WriteAsync(messageBuffer, 0, messageBuffer.Length, cancellationToken);
                    answer.Position = 0;
                    packetReader(answer, true, cancellationToken);
                }
            }, cancellationToken);
        }

        public Task Write(byte[] bytes, CancellationToken cancellationToken)
        {
            if (_client == null) return Task.FromResult(true);

            lock (LockObject)
            {
                if (cancellationToken.IsCancellationRequested) return Task.FromResult(true);
                try
                {
                    _stream.Write(bytes, 0, bytes.Length);
                }
                catch (IOException)
                {
                    //We have been disconnected from chromecast
                    //TODO: Raise an disconnected event
                }
            }

            return Task.FromResult(true);
        }

        public async Task Disconnect()
        {
            _stream.Dispose();
            _client.Dispose();
            _client = null;
        }
    }
}