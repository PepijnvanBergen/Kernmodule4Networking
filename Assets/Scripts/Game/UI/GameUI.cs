using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }
    [SerializeField]
    private GameObject gameUIMenu;
    [SerializeField]
    private GameObject onlineGameMenu;
    [SerializeField]
    private GameObject hostMenu;
    [SerializeField]
    private TMP_InputField addressInput;



    public TransportServer server;
    public TransportClient client;


    private void Awake()
    {
        Instance = this;
    }
    //MainMenu
    public void LocalGame()
    {
        server.Init(1511);
        client.Init("127.0.0.1", 1511);

        Debug.Log("LocalGame");
    }
    public void OnlineGame()
    {
        Debug.Log("OnlineGame");
        gameUIMenu.SetActive(false);
        onlineGameMenu.SetActive(true);
    }
    public void OnOnlineHost()
    {
        Debug.Log("OnOnlineHost");

        server.Init(1511);
        client.Init("127.0.0.1", 1511);

        onlineGameMenu.SetActive(false);
        hostMenu.SetActive(true);
    }
    public void OnOnlineConnect()
    {
        Debug.Log("addres " + addressInput);
        client.Init(addressInput.text, 1511);
        Debug.Log("OnOnlineConnect");
    }
    public void BackToMenu()
    {
        server.ShutDown();
        client.ShutDown();
        Debug.Log("OnOnlineBack");

        hostMenu.SetActive(false);
        onlineGameMenu.SetActive(false);
        gameUIMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
