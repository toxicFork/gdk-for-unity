using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;
#endif
using System;
using Improbable.Worker.CInterop;
using UnityEngine;


namespace Playground
{
    public class AndroidClientWorkerConnector : MobileWorkerConnector
    {
        public string IpAddress { get; set; }

        [SerializeField] private GameObject level;

        private GameObject levelInstance;
        
        // Fields needed to do cloud deployments
        [SerializeField] private bool useLocator;
        [SerializeField] private string projectName;
        [SerializeField] private string deploymentName;
        [SerializeField] private string loginToken;

        private void Start()
        {
            TryConnect();
        }

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

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        protected override string GetHostIp()
        {
#if UNITY_ANDROID
            if (!string.IsNullOrEmpty(IpAddress))
            {
                return IpAddress;
            }

            if (Application.isMobilePlatform && AndroidDeviceInfo.ActiveDeviceType == MobileDeviceType.Virtual)
            {
                return AndroidDeviceInfo.EmulatorDefaultCallbackIp;
            }

            return RuntimeConfigDefaults.ReceptionistHost;
#else
            throw new PlatformNotSupportedException(
                $"{nameof(AndroidClientWorkerConnector)} can only be used for the Android platform. Please check your build settings.");
#endif
        }
        
        protected override LocatorConfig GetLocatorConfig(string workerType)
        {
            return new LocatorConfig
            {
                LocatorParameters =
                {
                    CredentialsType = LocatorCredentialsType.LoginToken,
                    ProjectName = projectName,
                    LoginToken = new LoginTokenCredentials
                    {
                        Token = loginToken

                    }
                },
                WorkerType = workerType,
                WorkerId = CreateNewWorkerId(workerType),
                LinkProtocol = NetworkConnectionType.Kcp,
            };
        }

        protected override bool ShouldUseLocator()
        {
            return useLocator;
        }
        
        protected override string SelectDeploymentName(DeploymentList deployments)
        {
            return deploymentName;
        }

        protected override void OnDisconnected(string reason)
        {
            //Instantiate(this);
            base.OnDisconnected(reason);
        }

        public override void Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }

            base.Dispose();
        }
    }
}
