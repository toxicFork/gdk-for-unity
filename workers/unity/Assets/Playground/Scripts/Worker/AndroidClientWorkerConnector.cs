using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;
#endif
using System;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;


namespace Playground
{
    public class AndroidClientWorkerConnector : MobileWorkerConnector, IMobileClient
    {
        public string IpAddress { get; set; }
        public ConnectionScreenController ConnectionScreenController { get; set; }

        [SerializeField] private GameObject level;

        private GameObject levelInstance;

        protected override LocatorConfig GetLocatorConfig(string workerType)
        {
            return new LocatorConfig
            {
                LocatorParameters =
                {
                    CredentialsType = LocatorCredentialsType.LoginToken,
                    ProjectName = "unity_gdk",
                    LoginToken = new LoginTokenCredentials
                    {
                        Token =
                            "eyJhbGciOiJSUzM4NCIsImtpZCI6IjAwMDEifQ.eyJzdWIiOiJ1cmFudXMtYmFrZXJsb28tYnVsbGRvZ0Bhbm9ueW1vdXMuaW1wcm9iYWJsZS5pbyIsImRpc3BsYXlfbmFtZSI6ImJsdWUtZm91cnRlZW4taW5kaWEiLCJpYXQiOjE1Mzk3MDc4NDMsImV4cCI6MTUzOTcyOTQ0MywiYXBwX25hbWUiOiJ1bml0eV9nZGsiLCJkZXBsb3ltZW50X25hbWUiOiJ5b2xvbW9iaWxlY2xvdWQiLCJkZXBsb3ltZW50X3RhZyI6IiIsImlzcyI6Imh0dHBzOi8vbG9jYXRvci5pbXByb2JhYmxlLmlvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhdG9yLmltcHJvYmFibGUuaW8iLCJlbnYiOiJwcm9kdWN0aW9uIn0.oBQ10jeaXH2vXMCagTXD8oVpP54XjZU8VhdDwze2enUz1Ld2EjtglqVzNqXyMkcklEf3dYmSQxtO_VOTZSaopq3PUZasHaYKYbKK1ebD5nBnzQNiQayJGG4bAtShe7CC2GF9Q6e_L1YbL80NzsqMhOxkHsb9uW8iPVFnclo4a0reVPMYozcjYSbgS_eLh2aZ5FBFdJHJkJF_oNbhxN-Feu1o5-WC-mPg47_r6TV5n9tMkqhPuqDqDSVBmD_MWpXJvGG92nmRtWJ8UkGnHtyIfu3KV5gnfgynMFFwXgT0j5qrUFBjBZohC6h47MToEVVhyll7YWSMQhIpMfLzZmGslw"
                    }
                }
            };
        }

        protected override bool ShouldUseLocator()
        {
            return true;
        }

        protected override string SelectDeploymentName(DeploymentList deployments)
        {
            return "yolomobilespike";
        }

        public async void TryConnect()
        {
            await Connect(WorkerUtils.AndroidClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            ConnectionScreenController.OnSuccess();

            WorkerUtils.AddClientSystems(Worker.World);
            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform);
            levelInstance.transform.SetParent(null);
        }

        protected override void HandleWorkerConnectionFailure()
        {
            ConnectionScreenController.OnConnectionFailed();
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
