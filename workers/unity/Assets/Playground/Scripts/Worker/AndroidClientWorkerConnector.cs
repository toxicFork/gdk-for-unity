using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using UnityEngine;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;

#endif

namespace Playground
{
    public class AndroidClientWorkerConnector : MobileWorkerConnector
    {
        [NonSerialized] public string IpAddress;
        [NonSerialized] public ConnectionScreen ConnectionScreen;

        [SerializeField] private GameObject level;

        private GameObject levelInstance;

        public async void TryConnect()
        {
            await Connect(WorkerUtils.AndroidClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddClientSystems(Worker.World);
            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform);
            levelInstance.transform.SetParent(null);

            ConnectionScreen.OnSuccess();
        }

        protected override void HandleWorkerConnectionFailure()
        {
            ConnectionScreen.OnConnectionFailed();
        }

        protected override string GetHostIp()
        {
#if UNITY_ANDROID
            if (Application.isMobilePlatform && DeviceInfo.IsAndroidStudioEmulator() && IpAddress.Equals(string.Empty))
            {
                return DeviceInfo.AndroidStudioEmulatorDefaultCallbackIp;
            }

            return IpAddress;
#else
            throw new PlatformNotSupportedException(
                "This method is only defined for the Android platform. Please check your build settings.");
#endif
        }
    }
}
