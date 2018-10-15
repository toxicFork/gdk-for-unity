using Improbable.Gdk.Mobile;
using Playground.Worker;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    public class SplashScreenConnect : MonoBehaviour
    {
        [SerializeField] private GameObject workerPrefab;
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private InputField ipAddressInput;
        [SerializeField] private Button connectButton;
        [SerializeField] private Text errorMessage;

        protected string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

        public void Awake()
        {
            ipAddressInput.text = PlayerPrefs.GetString("cachedIp");
            connectButton.onClick.AddListener(Connect);
        }

        public void Connect()
        {
            var worker = Instantiate(workerPrefab);
            var connector = worker.GetComponent<MobileWorkerConnector>() as AndroidClientWorkerConnector;

            connector.ConnectNow();
        }
    }
}
