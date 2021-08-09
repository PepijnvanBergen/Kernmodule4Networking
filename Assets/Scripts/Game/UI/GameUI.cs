using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }
    [SerializeField]
    private GameObject gameUIMenu;
    [SerializeField]
    private GameObject hostConnectMenu;
    [SerializeField]
    private GameObject OnlineGame;
    [SerializeField]
    private GameObject waitingMenu;

    [SerializeField]
    private GameObject loginUserMenu;
    [SerializeField]
    private GameObject loginServerMenu;
    [SerializeField]
    private GameObject registerMenu;

    [Space]
    [SerializeField]
    private TMP_InputField addressInput;

    [Space]
    public TransportServer server;
    public TransportClient client;


    private void Awake()
    {
        Instance = this;
        RegisterToEvents();
    }
    //MainMenu
    public void OnPlay()
    {
        Debug.Log("PlayGame!");
        gameUIMenu.SetActive(false);
        hostConnectMenu.SetActive(true);
    }
    //hostConnectMenu
    public void onHost()
    {
        hostConnectMenu.SetActive(false);
        loginServerMenu.SetActive(true);
    }
    public void OnConnect()
    {
        hostConnectMenu.SetActive(false);
        loginUserMenu.SetActive(true);
    }
    public void OnRegister()
    {
        hostConnectMenu.SetActive(false);
        registerMenu.SetActive(true);
    }
    //Host
    public void OnOnlineHost()
    {
        Debug.Log("OnOnlineHost");

        server.Init(1511);
        client.Init("127.0.0.1", 1511);

        OnlineGame.SetActive(false);
        waitingMenu.SetActive(false);
    }
    //Connect
    public void OnOnlineConnect()
    {
        client.Init(addressInput.text, 1511);
        OnlineGame.SetActive(false);
        waitingMenu.SetActive(false);
    }
    //Back
    public void BackToMenu()
    {
        server.ShutDown();
        client.ShutDown();

        OnlineGame.SetActive(false);
        hostConnectMenu.SetActive(false);
        loginUserMenu.SetActive(false);
        loginServerMenu.SetActive(false);
        gameUIMenu.SetActive(true);
        registerMenu.SetActive(false);
        waitingMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        UnRegisterToEvents();
    }
    public void RegisterToEvents()
    {
        NetUtility.C_GAME_START += HideUI;
    }
    public void UnRegisterToEvents()
    {
        NetUtility.C_GAME_START -= HideUI;
    }
    private void HideUI(NetworkMessage msg)
    {
        OnlineGame.SetActive(false);
        hostConnectMenu.SetActive(false);
        gameUIMenu.SetActive(false);
        loginUserMenu.SetActive(false);
        loginServerMenu.SetActive(false);
    }
}
