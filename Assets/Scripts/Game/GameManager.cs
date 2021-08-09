using UnityEngine;
using Unity.Networking.Transport;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Board board;
    [SerializeField]
    private int bs;
    public static int boardSize;
    public static bool UIneeded;

    public int playerCount;
    public static int currentTeam;
    public static int score;

    //BackendVariables
    public static int player0ID;
    public static int player1ID;
    public static int serverID;

    private void Awake()
    {
        RegisterToEvents();
        UIneeded = true;
    }
    private void OnGameOverServer(NetworkMessage message, NetworkConnection cnn)
    {

    }
    private void OnGameOverClient(NetworkMessage message)
    {

    }
    private void OnWelcomeServer(NetworkMessage message, NetworkConnection cnn)
    {
        Welcome nw = message as Welcome;
        nw.assignedTeam = ++playerCount;
        TransportServer.Instance.SendToClient(cnn, nw);

        if(playerCount == 1f)
        {
            TransportServer.Instance.BroadCast(new StartGame());
            Debug.Log("We are starting the game!");
        }
    }
    private void OnWelcomeClient(NetworkMessage message)
    {
        Welcome nw = message as Welcome;
        currentTeam = nw.assignedTeam;
        Actions.myTeam = nw.assignedTeam;
        Debug.Log("My assigned team is " + currentTeam);
    }
    private void OnStartGameClient(NetworkMessage message)
    {
        boardSize = bs;
        board.Create();

        UIneeded = false;
        currentTeam = 1;
    }
    public void RegisterToEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_GAME_START += OnStartGameClient;
        NetUtility.C_GAME_IS_OVER += OnGameOverClient;
        NetUtility.S_GAME_IS_OVER += OnGameOverServer;
    }
    public void UnRegisterToEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_GAME_START -= OnStartGameClient;
        NetUtility.C_GAME_START -= OnGameOverClient;
        NetUtility.S_GAME_IS_OVER -= OnGameOverServer;
    }
    private void OnDestroy()
    {
        UnRegisterToEvents();
    }
}