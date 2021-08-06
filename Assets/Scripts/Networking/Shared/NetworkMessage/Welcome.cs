using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class Welcome : NetworkMessage
{
    public int assignedTeam { set; get; }
    public Welcome()
    {
        Event = GameEvent.WELCOME;
    }
    public Welcome(DataStreamReader reader)
    {
        Event = GameEvent.WELCOME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Event);
        writer.WriteInt(assignedTeam);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        assignedTeam = reader.ReadInt();
    }
    public override void RecievedOnClient()
    {
        NetUtility.C_WELCOME?.Invoke(this);
        Debug.Log(NetUtility.C_WELCOME);
    }
    public override void RecievedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_WELCOME?.Invoke(this, cnn);
    }
}
