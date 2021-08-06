using UnityEngine;
using System;
using Unity.Networking.Transport;
public enum GameEvent
{
    PLACE_PIECE = 1,
    SWITCH_TURN = 2,
    GAME_START = 3,
    GAME_IS_OVER = 4,
    GAME_CHATMESSAGE = 5,
    WELCOME = 6,
    REMATCH = 7,
    KEEP_ALIVE = 8
}
public static class NetUtility
{
    public static void OnData(DataStreamReader stream, NetworkConnection cnn, TransportServer server = null)
    {
        NetworkMessage msg = null;
        GameEvent gameEvent = (GameEvent)stream.ReadByte();
        switch (gameEvent)
        {
            case GameEvent.PLACE_PIECE: msg = new NetworkChatMessage(stream); break;
            case GameEvent.SWITCH_TURN: msg = new NetworkChatMessage(stream); break;
            case GameEvent.GAME_START: msg = new StartGame(stream); break;
            case GameEvent.GAME_IS_OVER: msg = new NetworkChatMessage(stream); break;
            case GameEvent.GAME_CHATMESSAGE: msg = new NetworkChatMessage(stream); break;
            case GameEvent.WELCOME: msg = new Welcome(stream); break;
            case GameEvent.REMATCH: msg = new NetworkChatMessage(stream); break;
            case GameEvent.KEEP_ALIVE: msg = new KeepAlive(stream); break;
            default:
                Debug.Log("Message recieved has no GameEvent "); break;
        }
        if (server != null)
        {
            msg.RecievedOnServer(cnn);
        }
        else
        {
            msg.RecievedOnClient();
        }
    }
    public static Action<NetworkMessage> C_PLACE_PIECE;
    public static Action<NetworkMessage> C_SWITCH_TURN;
    public static Action<NetworkMessage> C_GAME_START;
    public static Action<NetworkMessage> C_GAME_IS_OVER;
    public static Action<NetworkMessage> C_GAME_CHATMESSAGE;
    public static Action<NetworkMessage> C_WELCOME;
    public static Action<NetworkMessage> C_REMATCH;
    public static Action<NetworkMessage> C_KEEP_ALIVE;

    public static Action<NetworkMessage, NetworkConnection> S_PLACE_PIECE;
    public static Action<NetworkMessage, NetworkConnection> S_SWITCH_TURN;
    public static Action<NetworkMessage, NetworkConnection> S_GAME_START;
    public static Action<NetworkMessage, NetworkConnection> S_GAME_IS_OVER;
    public static Action<NetworkMessage, NetworkConnection> S_GAME_CHATMESSAGE;
    public static Action<NetworkMessage, NetworkConnection> S_WELCOME;
    public static Action<NetworkMessage, NetworkConnection> S_REMATCH;
    public static Action<NetworkMessage, NetworkConnection> S_KEEP_ALIVE;
}