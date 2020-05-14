using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;

public class ServerCommunication : MonoBehaviour
{
    [SerializeField] private int port = 5000;
    [SerializeField] private TextMeshProUGUI display = default;

    private string server;

    private WebSocketTestNamespace testNamespace;

    private void Awake()
    {
        server = "http://127.0.0.1:" + port;

        testNamespace = new WebSocketTestNamespace(server, display);
    }

    public void EmitTestMessage()
    {
        testNamespace.EmitTest();
    }

    public void EmitTestMessageJSON()
    {
        testNamespace.EmitTestJSON();
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
