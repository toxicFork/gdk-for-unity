using System;

[Serializable]
public class ClientRequest
{
    public int listenPort;

    public ClientRequest(int listenPort)
    {
        this.listenPort = listenPort;
    }
}
