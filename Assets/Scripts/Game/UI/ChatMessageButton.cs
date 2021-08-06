using UnityEngine;
using UnityEngine.UI;

public class ChatMessageButton : MonoBehaviour
{
    [SerializeField]
    private InputField chatInput;

    public void OnSubmitClick()
    {
        NetworkChatMessage msg = new NetworkChatMessage(chatInput.text);
        FindObjectOfType<TransportClient>().SendToServer(msg);
    }
}
