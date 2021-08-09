using UnityEngine;
using System;
using Unity.Networking.Transport;
public enum GameEvent
{
    PLACE_PIECE = 1,
    GAME_START = 2,
    GAME_IS_OVER = 3,
    WELCOME = 4,
    KEEP_ALIVE = 5
}
public static class NetUtility
{
    public static void OnData(DataStreamReader stream, NetworkConnection cnn, TransportServer server = null)
    {
        NetworkMessage msg = null;
        GameEvent gameEvent = (GameEvent)stream.ReadByte();
        switch (gameEvent)
        {
            case GameEvent.PLACE_PIECE: msg = new PlacePiece(stream); break;
            case GameEvent.GAME_START: msg = new StartGame(stream); break;
            case GameEvent.GAME_IS_OVER: msg = new GameIsOver(stream); break;
            case GameEvent.WELCOME: msg = new Welcome(stream); break;
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
    public static Action<NetworkMessage> C_GAME_START;
    public static Action<NetworkMessage> C_GAME_IS_OVER;
    public static Action<NetworkMessage> C_WELCOME;
    public static Action<NetworkMessage> C_KEEP_ALIVE;

    public static Action<NetworkMessage, NetworkConnection> S_PLACE_PIECE;
    public static Action<NetworkMessage, NetworkConnection> S_GAME_START;
    public static Action<NetworkMessage, NetworkConnection> S_GAME_IS_OVER;
    public static Action<NetworkMessage, NetworkConnection> S_WELCOME;
    public static Action<NetworkMessage, NetworkConnection> S_KEEP_ALIVE;
}