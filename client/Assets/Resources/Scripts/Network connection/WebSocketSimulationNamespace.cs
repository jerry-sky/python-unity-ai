using System;
using UnityEngine;
using UnityEngine.Events;
using socket.io;

[Serializable]
public class UnityEventString : UnityEvent<string>
{
}

[Serializable]
public class UnityEventBoard : UnityEvent<BoardUpdateModel>
{
}

[Serializable]
public class WebSocketSimulationNamespace
{
    private Socket socket;

    [NonSerialized] public UnityEvent onConnect = new UnityEvent();
    [NonSerialized] public UnityEvent<BoardUpdateModel> onUpdate = new UnityEventBoard();

    public void StartConnection(string serverUrl)
    {
        socket = Socket.Connect(serverUrl + "/simulation");

        socket.On("update",
            (string data) =>
            {
                Debug.Log(data);
                onUpdate.Invoke(JsonUtility.FromJson<BoardUpdateModel>(data));
            });

        socket.On(SystemEvents.connect, onConnect.Invoke);
    }

    public void StartSimulation(SimulationDataModel data)
    {
        string message = JsonUtility.ToJson(data);
        Debug.Log(message);
        // Can't send "
        message = message.Replace('\"', '\'');
        socket.Emit("start", message);
    }

}
