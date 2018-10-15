using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using UnityEngine;
using System;
using UnityEngine.UI;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;

#endif

namespace Playground
{
    public class AndroidClientWorkerConnector : MobileWorkerConnector
    {
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private GameObject level;
        [SerializeField] private InputField ipAddressInput;
        [SerializeField] private Button connectButton;
        [SerializeField] private Text errorMessage;

        private GameObject levelInstance;
        private bool connected;

        private string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

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
#if UNITY_ANDROID
            var hostIp = IpAddress;
            if (Application.isMobilePlatform && DeviceInfo.IsAndroidStudioEmulator() && hostIp.Equals(string.Empty))
            {
                return DeviceInfo.AndroidStudioEmulatorDefaultCallbackIp;
            }

            return hostIp;
#else
            return null;
#endif
        }
    }
}
