using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Improbable.GDK.EditorDiscovery
{
    internal class CancellablePacketReceiver
    {
        private readonly int packetReceiveTimeoutMs;
        private readonly IAsyncResult beginReceive;
        private readonly WaitHandle handle;
        private readonly UdpClient client;
        private readonly ManualResetEvent killTrigger;

        public enum ReceiveResult
        {
            Success,
            Cancelled,
            TimedOut
        }

        public CancellablePacketReceiver(UdpClient client, int packetReceiveTimeoutMs, ManualResetEvent killTrigger)
        {
            this.client = client;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.killTrigger = killTrigger;

            beginReceive = client.BeginReceive(null, null);
            handle = beginReceive.AsyncWaitHandle;
        }

        public ReceiveResult Poll(out IPEndPoint remoteEndPoint, out byte[] receivedBytes)
        {
            remoteEndPoint = null;
            receivedBytes = null;

            if (handle.WaitOne(packetReceiveTimeoutMs))
            {
                var remoteEp = new IPEndPoint(IPAddress.Any, 0);

                receivedBytes = client.EndReceive(beginReceive, ref remoteEp);

                remoteEndPoint = remoteEp;

                return ReceiveResult.Success;
            }

            if (killTrigger.WaitOne(0))
            {
                return ReceiveResult.Cancelled;
            }

            return ReceiveResult.TimedOut;
        }

        public void ForceEnd()
        {
            var remoteEp = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                client.EndReceive(beginReceive, ref remoteEp);
            }
            catch (ObjectDisposedException)
            {
                // https://stackoverflow.com/questions/18309974/how-do-you-cancel-a-udpclientbeginreceive
            }
        }
    }
}
