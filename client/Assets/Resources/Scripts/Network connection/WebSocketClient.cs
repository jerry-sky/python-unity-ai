using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

public class WebSocketClient
{
    private ClientWebSocket webSocket;
    private readonly UTF8Encoding encoder;
    private const UInt64 MAXREADSIZE = 1 * 1024 * 1024;

    private readonly Uri serverUri;

    public ConcurrentQueue<String> ReceiveQueue { get; }
    private BlockingCollection<ArraySegment<byte>> sendQueue;

    public WebSocketClient(string serverURL)
    {
        encoder = new UTF8Encoding();
        webSocket = new ClientWebSocket();

        serverUri = new Uri(serverURL);

        ReceiveQueue = new ConcurrentQueue<string>();
        var receiveThread = new Thread(RunReceive);
        receiveThread.Start();

        sendQueue = new BlockingCollection<ArraySegment<byte>>();
        var sendThread = new Thread(RunSend);
        sendThread.Start();
    }

    public async Task Connect()
    {
        Debug.Log("Connecting to: " + serverUri);
        await webSocket.ConnectAsync(serverUri, CancellationToken.None);
        while (IsConnecting())
        {
            Debug.Log("Waiting to connect...");
            Task.Delay(50).Wait();
        }
        Debug.Log("Connect status: " + webSocket.State);
    }

    #region Status
    public bool IsConnecting()
    {
        return webSocket.State == WebSocketState.Connecting;
    }

    public bool IsConnectionOpen()
    {
        return webSocket.State == WebSocketState.Open;
    }
    #endregion

    #region Send
    public void Send(string message)
    {
        byte[] buffer = encoder.GetBytes(message);
        var sendBuf = new ArraySegment<byte>(buffer);

        sendQueue.Add(sendBuf);
    }

    private async void RunSend()
    {
        ArraySegment<byte> msg;
        while (true)
        {
            while (!sendQueue.IsCompleted)
            {
                msg = sendQueue.Take();
                await webSocket.SendAsync(msg, WebSocketMessageType.Text, true , CancellationToken.None);
            }
        }
    }
    #endregion /Send

    #region Receive
    private async Task<string> Receive(UInt64 maxSize = MAXREADSIZE)
    {
        byte[] buf = new byte[4 * 1024];
        var ms = new MemoryStream();
        ArraySegment<byte> arrayBuf = new ArraySegment<byte>(buf);
        WebSocketReceiveResult chunkResult;

        if (IsConnectionOpen())
        {
            do
            {
                chunkResult = await webSocket.ReceiveAsync(arrayBuf, CancellationToken.None);
                ms.Write(arrayBuf.Array, arrayBuf.Offset, chunkResult.Count);
                if ((UInt64)chunkResult.Count > MAXREADSIZE)
                    Debug.LogError("Warning: Message is bigger than expected! " + chunkResult.Count);
            } while (!chunkResult.EndOfMessage);
            ms.Seek(0, SeekOrigin.Begin);

            // Looking for UTF-8 JSON type messages.
            if (chunkResult.MessageType == WebSocketMessageType.Text)
                return CommunicationUtils.StreamToString(ms, Encoding.UTF8);
        }
        return "";
    }

    private async void RunReceive()
    {
        string result;
        while (true)
        {
            result = await Receive();
            if (!string.IsNullOrEmpty(result))
                ReceiveQueue.Enqueue(result);
            else
                Task.Delay(50).Wait();
        }
    }
    #endregion
}
