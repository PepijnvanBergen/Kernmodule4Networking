using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

delegate void GameEventHandler(DataStreamReader stream, object sender, NetworkConnection connection);
public class TransportServer : MonoBehaviour
{
    #region Singleton implementation
    public static TransportServer Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    #region variables
    public NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private bool isActive = false;
    private const float keepAliveTickRate = 20.0f;
    private float lastKeepAlive;

    public Action connectionsDropped;
    #endregion

    public void Init(ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = port;//1511

        if (driver.Bind(endpoint) != 0)
        {
            Debug.Log("Failed to bind to port " + endpoint.Port);
            return;
        }
        else
        {
            driver.Listen(); //Vanaf nu kunnen we dingen ontvangen, de server luisterd naar spelers
            Debug.Log("Currently listening to port: " + endpoint.Port);
        }
        connections = new NativeList<NetworkConnection>(2, Allocator.Persistent); //De lijst met connections
        isActive = true;
    }
    public void ShutDown()
    {
        if (isActive)
        {
            driver.Dispose();
            connections.Dispose();
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
        Keepalive();
        CleanUpConnections();
        AcceptNewConnections();
        UpdateMessages();
        driver.ScheduleUpdate().Complete();
    }
    private void CleanUpConnections()//Clean up new connections
    {
        // Clean up connections
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }
    }
    private void AcceptNewConnections()//Accept new connections
    {
        // Accept new connections
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
            Debug.Log("Accepted a connection");
        }
    }
    private void UpdateMessages()//Listen and post new messages
    {
        //Messages!!
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
                continue;

            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                Debug.Log("server " + cmd);
                if (cmd == NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(stream, connections[i], this);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                    connectionsDropped?.Invoke();
                    ShutDown();

                }
                else
                {
                    //Unsupported event received...
                    Debug.Log("Unsupported event recieved");
                }
            }
        }
    }
    private void Keepalive()
    {
        if(Time.time - lastKeepAlive > keepAliveTickRate)
        {
            lastKeepAlive = Time.time;
            BroadCast(new KeepAlive());
        }
    }
    public void SendToClient(NetworkConnection connection, NetworkMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    public void BroadCast(NetworkMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].IsCreated)
            {
                SendToClient(connections[i], msg);
            }
        }
    }
}
