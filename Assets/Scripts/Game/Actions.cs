using UnityEngine;
using Unity.Networking.Transport;

public class Actions : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPiece;
    [SerializeField]
    private GameObject p1Piece;
    [SerializeField]
    private GameObject p2Piece;

    protected static GameObject workingPiece;

    public int currentTeam;
    public static int myTeam;

    [SerializeField]
    private Camera cam;

    private void Awake()
    {
        RegisterToEvents();
    }
    private void Start()
    {
        currentTeam = 1;
    }
    private void Update()
    {
        ChooseInputs();
    }
   private void OnPlacePieceServer(NetworkMessage nmsg, NetworkConnection cnn)
    {
        PlacePiece msg = nmsg as PlacePiece;
        TransportServer.Instance.BroadCast(msg);
    }
    private void ChooseInputs()//Hier moet wat mee gebeuren!!
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentTeam == myTeam)
            {
                if (currentTeam == 0)
                {
                    Cell cell = FindMouseCell(FindMouse(cam));
                    if (cell.piece != null) return;
                    PlacePiece pp = new PlacePiece();
                    pp.piece = 1;
                    pp.x = cell.x;
                    pp.y = cell.y;
                    pp.team = 0;
                    pp.nextTeam = 1;
                    TransportClient.Instance.SendToServer(pp);
                }
                else if (currentTeam == 1)
                {
                    Cell cell = FindMouseCell(FindMouse(cam));
                    if (cell.piece != null) return;
                    PlacePiece pp = new PlacePiece();
                    pp.piece = 1;
                    pp.x = cell.x;
                    pp.y = cell.y;
                    pp.team = 1;
                    pp.nextTeam = 0;
                    TransportClient.Instance.SendToServer(pp);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentTeam == myTeam)
            {
                if (currentTeam == 0)
                {
                    Cell cell = FindMouseCell(FindMouse(cam));
                    if (cell.piece != null) return;
                    PlacePiece pp = new PlacePiece();
                    pp.piece = 2;
                    pp.x = cell.x;
                    pp.y = cell.y;
                    pp.team = 0;
                    pp.nextTeam = 1;
                    TransportClient.Instance.SendToServer(pp);
                }
                else if (currentTeam == 1)
                {
                    Cell cell = FindMouseCell(FindMouse(cam));
                    if (cell.piece != null) return;
                    PlacePiece pp = new PlacePiece();
                    pp.piece = 2;
                    pp.x = cell.x;
                    pp.y = cell.y;
                    pp.team = 1;
                    pp.nextTeam = 0;
                    TransportClient.Instance.SendToServer(pp);
                }
            }
        }
    }
    private Vector2 FindMouse(Camera cam)
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        return mouseWorldPos;
    }
    private void OnPlacePieceClient(NetworkMessage nmsg)
    {
        PlacePiece msg = nmsg as PlacePiece;
        PlacePiece(msg.piece, msg.x, msg.y, msg.team);
        currentTeam = msg.nextTeam;
    }
    private void PlacePiece(int pieceNum, int x, int y, int pTeam)
    {
        Cell workCell = Board.cellBoard[x, y];
        if (pieceNum == 1)
        {
            if (pTeam == 0)
            {
                workingPiece = p1Piece;
            }
            else if (pTeam == 1)
            {
                workingPiece = p2Piece;
            }
        }
        else if (pieceNum == 2)
        {
            workingPiece = wallPiece;
        }
        PieceManager.piecesInt++;
        GameObject piece = Instantiate(workingPiece, workCell.pos, Quaternion.identity);
        PieceManager.piecesArray[workCell.x, workCell.y] = piece.GetComponent<BasePiece>();
        PieceManager.piecesArray[workCell.x, workCell.y].OnSpawn(workCell, pTeam, PieceManager.piecesInt, new Vector2(workCell.x, workCell.y));
        CheckIfWon(PieceManager.piecesArray, PieceManager.piecesArray[workCell.x, workCell.y]);
    }
    private Cell FindMouseCell(Vector2 position)
    {
        foreach (Cell cell in Board.cellBoard)
        {
            if (Vector2.Distance(cell.pos, position) < 0.4999f)
            {
                return cell;
            }
        }
        Debug.Log("ERROR No close cell has been found error!");
        return null;
    }
    private void CheckIfWon(BasePiece[,] wPA, BasePiece wPiece)
    {
        int numInRow = 1;

        int x = wPiece.x;
        int y = wPiece.y;
        int bs = GameManager.boardSize - 1;
        //checking the x-axis
        {
            if (x != 0 && wPA[x - 1, y] != null && wPA[x - 1, y].team == wPiece.team)
            {
                Debug.Log("One to the left");
                numInRow++;
            }
            if (x != 1 && x != 0 && wPA[x - 2, y] != null && wPA[x - 2, y].team == wPiece.team)
            {
                Debug.Log("One two to the left");
                numInRow++;
            }
            if (x != bs && wPA[x + 1, y] != null && wPA[x + 1, y].team == wPiece.team)
            {
                Debug.Log("One to the right!");
                numInRow++;
            }
            if (x != bs - 1 && x != bs && wPA[x + 2, y] != null && wPA[x + 2, y].team == wPiece.team)
            {
                Debug.Log("One two to the right");
                numInRow++;
            }
            if (numInRow == 3)
            {
                Debug.Log("Team " + wPiece.team + " wins!");
                GameIsOver gio = new GameIsOver();
                gio.score = PieceManager.piecesInt;
                gio.winningTeam = myTeam;
                TransportClient.Instance.SendToServer(gio);
            }
            else
            {
                numInRow = 1;
            }
        }
        //checking the y-axis
        {
            if (y != 0 && wPA[x, y - 1] != null && wPA[x, y - 1].team == wPiece.team)
            {
                Debug.Log("One to bottom");
                numInRow++;
            }
            if (y != 1 && y != 0 && wPA[x, y - 2] != null && wPA[x, y - 2].team == wPiece.team)
            {
                Debug.Log("One two to the bottom");
                numInRow++;
            }
            if (y != bs && wPA[x, y + 1] != null && wPA[x, y + 1].team == wPiece.team)
            {
                Debug.Log("One to the up");
                numInRow++;
            }
            if (y != bs - 1 && y != bs && wPA[x, y + 2] != null && wPA[x, y + 2].team == wPiece.team)
            {
                Debug.Log("One two to the up");
                numInRow++;
            }
            if (numInRow == 3)
            {
                Debug.Log("Team " + wPiece.team + " wins!");
                GameIsOver gio = new GameIsOver();
                gio.score = PieceManager.piecesInt;
                gio.winningTeam = myTeam;
                TransportClient.Instance.SendToServer(gio);
            }
            else
            {
                numInRow = 1;
            }
        }
    }
    private void OnGameIsOverServer(NetworkMessage nmsg, NetworkConnection cnn)
    {
        GameIsOver msg = nmsg as GameIsOver;
        TransportServer.Instance.BroadCast(msg);
    }
    private void OnGameIsOverClient(NetworkMessage nmsg)
    {
        GameIsOver msg = nmsg as GameIsOver;
        GameManager.score = msg.score;
        StartCoroutine(Playerlogin.SendScore(Playerlogin.sessionID, msg.score));
        Application.Quit();
    }
    public void RegisterToEvents()
    {
        NetUtility.S_PLACE_PIECE += OnPlacePieceServer;
        NetUtility.C_PLACE_PIECE += OnPlacePieceClient;
        NetUtility.S_GAME_IS_OVER += OnGameIsOverServer;
        NetUtility.C_GAME_IS_OVER += OnGameIsOverClient;

    }
    public void UnRegisterToEvents()
    {
        NetUtility.S_PLACE_PIECE -= OnPlacePieceServer;
        NetUtility.C_PLACE_PIECE -= OnPlacePieceClient;
        NetUtility.S_GAME_IS_OVER -= OnGameIsOverServer;
        NetUtility.C_GAME_IS_OVER -= OnGameIsOverClient;
    }
    private void OnDestroy()
    {
        UnRegisterToEvents();
    }
}