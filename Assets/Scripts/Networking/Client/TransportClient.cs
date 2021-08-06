using System;
using UnityEngine;
using Unity.Networking.Transport;

public class TransportClient : MonoBehaviour
{
    #region Singleton implementation
    public static TransportClient Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public NetworkDriver driver;
    public NetworkConnection connection;

    private bool isActive = false;

    public Action connectionDropped;

    public void Init(string ip, ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);
        endpoint.Port = port;//1511

        connection = driver.Connect(endpoint);
        Debug.Log("Attempting to connect to Server on " + endpoint.Address);

        isActive = true;
        RegisterToEvent();
    }

    public void ShutDown()
    {
        if (isActive)
        {
            UnRegisterToEvent();
            driver.Dispose();
            connection = default(NetworkConnection);
            isActive = false;
        }
    }
    private void OnDestroy()
    {
        ShutDown();
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessages();
    }
    private void UpdateMessages()//Listen and post new messages
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;

        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            Debug.Log("client " + cmd);
            if(cmd == NetworkEvent.Type.Connect)
            {
                SendToServer(new Welcome());
                Debug.Log("We are connected!");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NetUtility.OnData(stream, connection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
                ShutDown();
            }
            else
            {
                //Unsupported event received...
                Debug.Log("Unsupported event recieved");
            }
        }
        
    }
    private void CheckAlive()
    {
        if(!connection.IsCreated && isActive)
        {
            Debug.Log("Something went wrong, lost connection to server!");
            connectionDropped?.Invoke();
            ShutDown();
        }
    }
    public void SendToServer(NetworkMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    private void OnKeepAlive(NetworkMessage nmg)
    {
        SendToServer(nmg);
        Debug.Log("Client sending Keeping alive!");
    }
    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }
    private void UnRegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }
}