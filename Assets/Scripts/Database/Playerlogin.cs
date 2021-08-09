using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using Unity.Networking.Transport;
public class Playerlogin : MonoBehaviour
{
    [SerializeField]
    private TMP_Text highscores;
    [SerializeField]
    private TMP_InputField userIDInput;
    [SerializeField]
    private TMP_InputField passwordInput;

    [Space]
    [SerializeField]
    private TMP_InputField serverIDInput;
    [SerializeField]
    private TMP_InputField serverpasswordInput;

    [Space]
    [SerializeField]
    private TMP_InputField serverUserIDInput;
    [SerializeField]
    private TMP_InputField serverPasswordInput;
    [SerializeField]
    private TMP_InputField SessionIDServer;
    [SerializeField]
    private TMP_InputField sessionIDUser;

    [Space]
    [SerializeField]
    private TMP_InputField registerName;
    [SerializeField]
    private TMP_InputField registerPassword;
    [SerializeField]
    private TMP_InputField registerEmail;
    [SerializeField]
    private TMP_Text registerOutput;

    [Space]
    [SerializeField]
    private TMP_Text outputField;

    [Space]
    [SerializeField]
    private Button serverLoginButton;
    [SerializeField]
    private Button userLoginButton;
    [SerializeField]
    private Button user2LoginButton;

    [Space]
    [SerializeField]
    private GameObject OnlineGameUI;
    [SerializeField]
    private GameObject serverUI;
    [SerializeField]
    private GameObject userUI;

    public static string sessionID;
    private void Start()
    {
        StartCoroutine(GetHighScores());
    }
    public void OnRegister()
    {
        StartCoroutine(InsertUser(registerName.text, registerEmail.text, registerPassword.text));// 3 - DonDon
        userLoginButton.interactable = false;
    }
    public void OnConnectUser()
    {
        int id = 0;
        int.TryParse(userIDInput.text, out id);
        StartCoroutine(LoginUser(id, passwordInput.text, false, sessionIDUser.text));// 3 - DonDon
        userLoginButton.interactable = false;
        sessionID = sessionIDUser.text;
        GameManager.player1ID = id;
    }
    public void OnConnectUser2()
    {
        int id = 0;
        int.TryParse(userIDInput.text, out id);
        StartCoroutine(LoginUser(id, serverPasswordInput.text, true, SessionIDServer.text)); // 3 - DonDon - SessionID
        user2LoginButton.interactable = false;
        sessionID = sessionIDUser.text;
        GameManager.player0ID = id;
        sessionID = SessionIDServer.text;
    }
    public void OnConnectedServer()
    {
        StartCoroutine(LoginServer(serverIDInput.text, serverpasswordInput.text));// 2 - WachtwoordWachtwoord2
        serverLoginButton.interactable = false;
    }

    private void OnGameOver(NetworkMessage message, NetworkConnection cnn)
    {
        StartCoroutine(SendScore(sessionID, GameManager.score));
    }
    IEnumerator GetHighScores()
    {
        using(UnityWebRequest www = UnityWebRequest.Get("https://studenthome.hku.nl/~pepijn.vanbergen/Database/average_score.php?"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string result = www.downloadHandler.text;
                highscores.text = result;
            }
        }
    }
    IEnumerator LoginUser(int ID, string password, bool one, string sessionID)
    {
        string url = "https://studenthome.hku.nl/~pepijn.vanbergen/Database/user_login.php?PHPSESSID=" + sessionID + "id=" + ID + "&pw=" + password;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            Debug.Log("try again something was wrong");
            serverLoginButton.interactable = true;
        }
        else
        {
            string result = www.downloadHandler.text;
            outputField.text = "Welcome " + result;
            if (one)
            {
                serverUI.SetActive(false);
                userUI.SetActive(false);
            }
            else
            {
                serverUI.SetActive(false);
                userUI.SetActive(false);
            }

            OnlineGameUI.SetActive(true);
        }
    }
    IEnumerator LoginServer(string ID, string password)
    {
        string url = "https://studenthome.hku.nl/~pepijn.vanbergen/Database/server_login.php?id=" + ID + "&pw=" + password;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            Debug.Log("try again something was wrong");
            userLoginButton.interactable = true;
            user2LoginButton.interactable = true;
        }
        else
        {
            string result = www.downloadHandler.text;
            outputField.text = "Your Session ID = " + result;
        }
    }
    IEnumerator InsertUser(string name, string email, string password)
    {
        string url = "https://studenthome.hku.nl/~pepijn.vanbergen/Database/player_insert.php?naam=" + name + "&email=" + email + "&wachtwoord=" + password;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            Debug.Log("try again something was wrong");
            userLoginButton.interactable = true;
        }
        else
        {
            string result = www.downloadHandler.text;
            Debug.Log("Your login ID = " + result + ".");
            registerOutput.text = "Your login ID = " + result + ". Use this and your password to log in.";
        }
    }
    public static IEnumerator SendScore(string sessionID, int score)
    {
        Debug.Log("hi");
        string url = "https://studenthome.hku.nl/~pepijn.vanbergen/Database/score_insert.php?PHPSESSID=" + sessionID + "&score=" + score;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
    }
    public void RegisterToEvents()
    {
        NetUtility.S_GAME_IS_OVER += OnGameOver;
    }
}