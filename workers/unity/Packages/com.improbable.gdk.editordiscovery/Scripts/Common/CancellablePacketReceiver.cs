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

        public enum PollResult
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

        public PollResult Poll(out IPEndPoint remoteEndPoint, out byte[] receivedBytes)
        {
            remoteEndPoint = null;
            receivedBytes = null;

            try
            {
                if (handle.WaitOne(packetReceiveTimeoutMs))
                {
                    var remoteEp = new IPEndPoint(IPAddress.Any, 0);

                    receivedBytes = client.EndReceive(beginReceive, ref remoteEp);

                    remoteEndPoint = remoteEp;

                    return PollResult.Success;
                }

                if (killTrigger.WaitOne(0))
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
                client.EndReceive(beginReceive, ref remoteEp);
            }
            catch (ObjectDisposedException)
            {
                // https://stackoverflow.com/questions/18309974/how-do-you-cancel-a-udpclientbeginreceive
            }
        }
    }
}
