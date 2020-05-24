using UnityEngine;

public class ServerCommunication : MonoBehaviour
{
    [SerializeField] private int port = 5000;
    public WebSocketSimulationNamespace simulationNamespace = new WebSocketSimulationNamespace();

    private string server;

    private void Awake()
    {
        server = "http://127.0.0.1:" + port;

        simulationNamespace.StartConnection(server);
    }
}
