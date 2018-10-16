using Improbable.Gdk.Mobile;
using Playground;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionScreen : MonoBehaviour
{
    private const string CachedIp = "cachedIp";

    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private InputField ipAddressInput;
    [SerializeField] private Button connectButton;
    [SerializeField] private Text errorMessage;
    [SerializeField] private GameObject prefab;

    private string IpAddress => ipAddressInput != null ? ipAddressInput.text : null;

    public void Awake()
    {
        ipAddressInput.text = PlayerPrefs.GetString(CachedIp);
        connectButton.onClick.AddListener(TryConnect);
    }

    private void TryConnect()
    {
        var worker = Instantiate(prefab);
        var workerConnector = worker.GetComponent<MobileWorkerConnector>();

        switch (workerConnector)
        {
            case AndroidClientWorkerConnector _:
                var androidConnector = workerConnector as AndroidClientWorkerConnector;
                if (androidConnector != null)
                {
                    androidConnector.IpAddress = IpAddress;
                    androidConnector.ConnectionScreen = this;
                    androidConnector.TryConnect();
                }

                break;
            case iOSClientWorkerConnector _:
                var iOSConnector = workerConnector as iOSClientWorkerConnector;
                iOSConnector.IpAddress = IpAddress;
                iOSConnector.ConnectionScreen = this;
                break;
        }
    }

    public void OnSuccess()
    {
        PlayerPrefs.SetString(CachedIp, IpAddress);
        PlayerPrefs.Save();

        connectionPanel.SetActive(false);
    }

    public void OnConnectionFailed()
    {
        errorMessage.text = "Connection failed. Please check the IP address entered.";
    }
}
