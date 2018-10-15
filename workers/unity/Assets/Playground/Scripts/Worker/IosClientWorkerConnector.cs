using System;
using UnityEngine;
#if UNITY_IOS
using Improbable.Gdk.Mobile.Ios;
#endif
using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using UnityEngine.UI;

namespace Playground
{
    public class iOSClientWorker : MobileWorkerConnector
    {
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private GameObject level;
        [SerializeField] private InputField ipAddressInput;
        [SerializeField] private Button connectButton;
        [SerializeField] private Text errorMessage;

        private GameObject levelInstance;
        private bool connected;

        protected string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

        public void Awake()
        {
            ipAddressInput.text = PlayerPrefs.GetString("cachedIp");
            connectButton.onClick.AddListener(Connect);
        }

        public async void Connect()
        {
            errorMessage.text = "";
            await Connect(WorkerUtils.iOSClient, new ForwardingDispatcher()).ConfigureAwait(false);
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

            connected = true;
            connectionPanel.SetActive(false);

            PlayerPrefs.SetString("cachedIp", IpAddress);
            PlayerPrefs.Save();
        }

        protected override void HandleWorkerConnectionFailure()
        {
            errorMessage.text = "Connection failure. Please check the IP address";
        }

        public override void Dispose()
        {
            if (connected)
            {
                base.Dispose();
            }
        }

        protected override string GetHostIp()
        {
#if UNITY_IOS
            var hostIp = IpAddress;
            if ((Application.isEditor || DeviceInfo.IsIosSimulator()) && hostIp.Equals(string.Empty))
            {
                return RuntimeConfigDefaults.ReceptionistHost;
            }

            return hostIp;
#else
            throw new NotImplementedException("Incompatible platform: Please use iOS");
#endif
        }
    }
}
