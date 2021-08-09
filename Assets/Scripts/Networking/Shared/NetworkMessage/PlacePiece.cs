using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class PlacePiece : NetworkMessage
{
    public int piece;
    public int x;
    public int y;
    public int team;
    public int nextTeam;
    public PlacePiece()
    {
        Event = GameEvent.PLACE_PIECE;
    }
    public PlacePiece(DataStreamReader reader)
    {
        Event = GameEvent.PLACE_PIECE;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Event);
        writer.WriteInt(piece);
        writer.WriteInt(x);
        writer.WriteInt(y);
        writer.WriteInt(team);
        writer.WriteInt(nextTeam);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        piece = reader.ReadInt();
        x = reader.ReadInt();
        y = reader.ReadInt();
        team = reader.ReadInt();
        nextTeam = reader.ReadInt();
    }
    public override void RecievedOnClient()
    {
        NetUtility.C_PLACE_PIECE?.Invoke(this);
        Debug.Log(NetUtility.C_PLACE_PIECE);
    }
    public override void RecievedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_PLACE_PIECE?.Invoke(this, cnn);
    }
}
