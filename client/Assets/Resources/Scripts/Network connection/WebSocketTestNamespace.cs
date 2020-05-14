using UnityEngine;
using socket.io;
using TMPro;

public class WebSocketTestNamespace
{
    private Socket socket;
    private TextMeshProUGUI textField;

    public WebSocketTestNamespace(string server, TextMeshProUGUI textField)
    {
        this.textField = textField;
        // news namespace
        socket = Socket.Connect(server + "/test");

        socket.On(SystemEvents.connect, () =>
        {
            textField.text = "Connected with test websocket";
        });
    }

    public void EmitTest()
    {
        socket.On("my_response", (string data) => { textField.text = data; });
        socket.Emit("my_event", "some data");
    }

    public void EmitTestJSON()
    {
        textField.text = "Reading from JSON object \"data\":\n";
        socket.On("my_response", (string data) =>
        {
            textField.text += JsonUtility.FromJson<WebsocketTestModel>(data).data;
        });
        socket.Emit("my_event", "some data");
    }
}


