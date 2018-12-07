using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Improbable.GDK.EditorDiscovery
{
    internal class CancellablePacketReceiver
    {
        private readonly int _packetReceiveTimeoutMs;
        private readonly IAsyncResult _beginReceive;
        private readonly WaitHandle _handle;
        private readonly UdpClient _client;
        private readonly ManualResetEvent _killTrigger;

        public enum PollResult
        {
            Success,
            Cancelled,
            TimedOut
        }

        public CancellablePacketReceiver(UdpClient client, int packetReceiveTimeoutMs, ManualResetEvent killTrigger)
        {
            _client = client;
            _packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            _killTrigger = killTrigger;

            _beginReceive = client.BeginReceive(null, null);
            _handle = _beginReceive.AsyncWaitHandle;
        }

        public PollResult Poll(out IPEndPoint remoteEndPoint, out byte[] receivedBytes)
        {
            remoteEndPoint = null;
            receivedBytes = null;

            try
            {
                if (_handle.WaitOne(_packetReceiveTimeoutMs))
                {
                    var remoteEp = new IPEndPoint(IPAddress.Any, 0);

                    receivedBytes = _client.EndReceive(_beginReceive, ref remoteEp);

                    remoteEndPoint = remoteEp;

                    return PollResult.Success;
                }

                if (_killTrigger.WaitOne(0))
                {
                    return PollResult.Cancelled;
                }

                return PollResult.TimedOut;
            }
            catch (ThreadAbortException)
            {
                return PollResult.Cancelled;
            }
        }

        public void ForceEnd()
        {
            var remoteEp = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                _client.EndReceive(_beginReceive, ref remoteEp);
            }
            catch (ObjectDisposedException)
            {
                // https://stackoverflow.com/questions/18309974/how-do-you-cancel-a-udpclientbeginreceive
            }
        }
    }
}
