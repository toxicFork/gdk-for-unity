using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_IOS
using Improbable.Gdk.Mobile.iOS;
#endif
using System;
using Improbable.Worker.CInterop;
using UnityEditor;
using UnityEngine;

namespace Playground
{
    public class iOSClientWorkerConnector : MobileWorkerConnector
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
            await Connect(WorkerUtils.iOSClient, new ForwardingDispatcher()).ConfigureAwait(false);
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
#if UNITY_IOS
            if (!string.IsNullOrEmpty(IpAddress))
            {
                return IpAddress;
            }

            return RuntimeConfigDefaults.ReceptionistHost;
#else
            throw new PlatformNotSupportedException(
                $"{nameof(iOSClientWorkerConnector)} can only be used for the iOS platform. Please check your build settings.");
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
