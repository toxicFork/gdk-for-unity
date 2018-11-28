using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Mobile
{
    public abstract class MobileWorkerConnector : WorkerConnector
    {
        protected abstract string GetHostIp();

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = GetHostIp(),
                WorkerType = workerType,
                WorkerId = CreateNewWorkerId(workerType),
                UseExternalIp = true,
                LinkProtocol = NetworkConnectionType.Kcp,
            };
        }
    }
}
