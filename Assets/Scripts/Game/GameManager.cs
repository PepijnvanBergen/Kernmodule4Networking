using UnityEngine;
using Unity.Networking.Transport;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Board board;
    [SerializeField]
    private int bs;
    public static int boardSize;

    public int playerCount = -1;
    public int currentTeam = -1;

    private void Awake()
    {
        RegisterToEvents();
    }
    public void RegisterToEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_GAME_START += OnStartGameClient;
    }
    public void UnRegisterToEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.C_WELCOME -= OnWelcomeClient;
    }
    private void OnWelcomeServer(NetworkMessage message, NetworkConnection cnn)
    {
        Welcome nw = message as Welcome;

        nw.assignedTeam = ++playerCount;
        TransportServer.Instance.SendToClient(cnn, nw);

        if(playerCount == 0)
        {
            TransportServer.Instance.BroadCast(new StartGame());
            Debug.Log("We are starting the game!");

            boardSize = bs;
            board.Create();
        }
    }
    public void OnWelcomeClient(NetworkMessage message)
    {
        Welcome nw = message as Welcome;
        currentTeam = nw.assignedTeam;

        Debug.Log("My assigned team is " + currentTeam);
    }
    public void OnStartGameClient(NetworkMessage message)
    {

    }
    private void OnDestroy()
    {
        UnRegisterToEvents();
    }
}