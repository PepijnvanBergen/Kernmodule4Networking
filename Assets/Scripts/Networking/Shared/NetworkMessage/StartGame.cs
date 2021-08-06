using Unity.Networking.Transport;

public class StartGame : NetworkMessage
{
    public StartGame()
    {
        Event = GameEvent.GAME_START;
    }
    public StartGame(DataStreamReader reader)
    {
        Event = GameEvent.GAME_START;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Event);
    }
    public override void Deserialize(DataStreamReader reader)
    {
    }
    public override void RecievedOnClient()
    {
        NetUtility.C_GAME_START?.Invoke(this);
    }
    public override void RecievedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_GAME_START?.Invoke(this, cnn);
    }
}
