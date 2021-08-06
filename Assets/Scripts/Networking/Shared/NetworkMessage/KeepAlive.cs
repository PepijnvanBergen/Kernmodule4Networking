using UnityEngine;
using Unity.Networking.Transport;

public class KeepAlive : NetworkMessage
{
    public KeepAlive()
    {
        Event = GameEvent.KEEP_ALIVE;
    }
    public KeepAlive(DataStreamReader reader)
    {
        Event = GameEvent.KEEP_ALIVE;
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
        NetUtility.C_KEEP_ALIVE?.Invoke(this);
    }
    public override void RecievedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_KEEP_ALIVE?.Invoke(this, cnn);
    }
}
