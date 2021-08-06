using UnityEngine;
using Unity.Networking.Transport;

public class NetworkMessage
{
    public GameEvent Event { set; get; }

    public virtual void Serialize(ref DataStreamWriter writer)
    {

    }
    public virtual void Deserialize(DataStreamReader reader)
    {

    }
    public virtual void RecievedOnClient()
    {

    }
    public virtual void RecievedOnServer(NetworkConnection cnn)
    {
    
    }
}
