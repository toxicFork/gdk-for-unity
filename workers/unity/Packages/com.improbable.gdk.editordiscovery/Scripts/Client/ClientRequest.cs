using System;

[Serializable]
public class ClientRequest
{
    public int ListenPort;

    public ClientRequest(int listenPort)
    {
        ListenPort = listenPort;
    }
}
