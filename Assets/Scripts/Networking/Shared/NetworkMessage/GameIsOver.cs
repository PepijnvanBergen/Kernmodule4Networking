using Unity.Networking.Transport;

public class GameIsOver : NetworkMessage
{
    public int score;
    public int winningTeam;
    public GameIsOver()
    {
        Event = GameEvent.GAME_IS_OVER;
    }
    public GameIsOver(DataStreamReader reader)
    {
        Event = GameEvent.GAME_IS_OVER;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Event);
        writer.WriteInt(score);
        writer.WriteInt(winningTeam);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        score = reader.ReadInt();
        winningTeam = reader.ReadInt();
    }
    public override void RecievedOnClient()
    {
        NetUtility.C_GAME_IS_OVER?.Invoke(this);
    }
    public override void RecievedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_GAME_IS_OVER?.Invoke(this, cnn);
    }
}