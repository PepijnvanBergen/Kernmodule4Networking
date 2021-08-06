using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class NetworkPlayerPosition : NetworkMessage
{
    public int playerId { set; get; }
    public float positionX { set; get; }
    public float positionY { set; get; }
    public float positionZ { set; get; }

    public NetworkPlayerPosition()
    {
        //Event = GameEvent.POSITION;
    }
    public NetworkPlayerPosition(DataStreamReader reader)
    {
        //Event = GameEvent.POSITION;
        Deserialize(reader);
    }
    public NetworkPlayerPosition(int pId, float x, float y, float z)
    {
        //Event = GameEvent.POSITION;
        playerId = pId;
        positionX = x;
        positionY = y;
        positionZ = z;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Event);
        writer.WriteInt(playerId);
        writer.WriteFloat(positionX);
        writer.WriteFloat(positionY);
        writer.WriteFloat(positionZ);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        //De eerste byte (bericht) is al gehandeled
        playerId = reader.ReadInt();
        positionX = reader.ReadInt();
        positionY = reader.ReadInt();
        positionZ = reader.ReadInt();
    }
}