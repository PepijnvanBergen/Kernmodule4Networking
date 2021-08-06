using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class NetworkChatMessage : NetworkMessage
{
    public FixedString128 chatMessage { set; get; }

    public NetworkChatMessage()
    {
        Event = GameEvent.GAME_CHATMESSAGE;
    }
    public NetworkChatMessage(DataStreamReader reader)
    {
        Event = GameEvent.GAME_CHATMESSAGE;
        Deserialize(reader);
    }
    public NetworkChatMessage(string msg)
    {
        Event = GameEvent.GAME_CHATMESSAGE;
        chatMessage = msg;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Event);
        writer.WriteFixedString128(chatMessage);
    }
    public override void Deserialize( DataStreamReader reader)
    {
        //De eerste byte (bericht) is al gehandeled
        chatMessage = reader.ReadFixedString128();
    }
    //public override void RecievedOnServer(TransportServer server)
    //{
    //    Debug.Log("Recieved message on Server: " + chatMessage);
    //}
    public override void RecievedOnClient()
    {
        Debug.Log("Recieved message on Client: " + chatMessage);
    }
}