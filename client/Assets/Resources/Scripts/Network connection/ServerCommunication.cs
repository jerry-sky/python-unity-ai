using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class ServerCommunication : MonoBehaviour
{
    [SerializeField] private int port = 5000;

    private string server;
    private WebSocketClient client;

    private void Awake()
    {
        server = "http://127.0.0.1:" + port;
        client = new WebSocketClient(server);
    }

    private void Update()
    {
        var cqueue = client.ReceiveQueue;
        string msg;
        while (cqueue.TryPeek(out msg))
        {
            cqueue.TryDequeue(out msg);
            HandleMessage(msg);
        }
    }

    private void HandleMessage(string msg)
    {
        Debug.Log("Server: " + msg);
        var message = JsonUtility.FromJson<MessageModel>(msg);
        switch (message.method)
        {
            case "test":
                //Lobby.OnConnectedToServer?.Invoke();
                Debug.Log("Interpreted text: " + message.message);
                break;
            case "JsonInMessage":
                //DoSth(JsonUtility.FromJson<EchoMessageModel>(message.message));
                break;
            default:
                Debug.LogError("Unknown type of method: " + message.method);
                break;
        }
    }

    public async void ConnectToServer()
    {
        await client.Connect();
    }

    public void SendSocketMessage(string message)
    {
        client.Send(message);
    }


    #region HttpRequest
    public string Get(string requestPath)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server + requestPath);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using(Stream stream = response.GetResponseStream())
        using(StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    public string Post(string requestPath, string data, string contentType, string method = "POST")
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server + requestPath);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        request.ContentLength = dataBytes.Length;
        request.ContentType = contentType;
        request.Method = method;

        using(Stream requestBody = request.GetRequestStream())
        {
            requestBody.Write(dataBytes, 0, dataBytes.Length);
        }

        using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using(Stream stream = response.GetResponseStream())
        using(StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
    #endregion /HttpRequest
}
