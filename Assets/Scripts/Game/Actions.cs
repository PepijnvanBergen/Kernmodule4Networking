using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Actions : MonoBehaviour
{
    [SerializeField]
    private GameObject sp;
    [SerializeField]
    private GameObject p1p;
    [SerializeField]
    private GameObject p2p;
    [SerializeField]
    protected static GameObject wallPiece;
    [SerializeField]
    protected static GameObject p1Piece;
    [SerializeField]
    protected static GameObject p2Piece;

    protected static GameObject workingPiece;

    public static bool hasDoneAction;
    public static int[,] scoreArray = new int[5, 5];
    public  int[,] scorearray = new int[5, 5];
    public int boardSize = 5;

    private void Start()
    {
        p1Piece = p1p;
        p2Piece = p2p;
        wallPiece = sp;

        hasDoneAction = false;
        for(int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                scoreArray[x, y] = 0;
            }
        }
    }
    private void Update()
    {
        scorearray = scoreArray;
    }
    public static void PlacePiece(Cell wCell, int pTurn, int move)//Hier moet denk ik een network reference naar de speler komen
    {
        Cell workCell = wCell;
        if (workCell.piece == null)
        {
            if (move == 1)
            {
                if (pTurn == 1)
                {
                    workingPiece = p1Piece;
                }
                else if (pTurn == 2)
                {
                    workingPiece = p2Piece;
                }
            }
            else if (move == 2)
            {
                workingPiece = wallPiece;
            }
            PieceManager.piecesInt++;
            GameObject piece = Instantiate(workingPiece, workCell.pos, Quaternion.identity);
            PieceManager.piecesArray[workCell.x, workCell.y] = piece.GetComponent<BasePiece>();
            PieceManager.piecesArray[workCell.x, workCell.y].OnSpawn(workCell, pTurn, PieceManager.piecesInt, new Vector2(workCell.x, workCell.y));
            if(pTurn == 1)
            {
                scoreArray[workCell.x, workCell.y] = 1;
            }
            else if(pTurn == 2)
            {
                scoreArray[workCell.x, workCell.y] = 2;
            }
            CheckIfWon(PieceManager.piecesArray, PieceManager.piecesArray[workCell.x, workCell.y]);
            hasDoneAction = true;
        }
        else
        {
            Debug.Log("Cell is full try something else, turn isn't wasted yet!");
            Debug.Log("The piece resting on this cell is belonging to team: " + workCell.piece.team);
            return;
        }
    }
    public static Cell FindMouseCell(Vector2 position)
    {
        foreach (Cell cell in Board.cellBoard)
        {
            if(Vector2.Distance(cell.pos, position) < 0.4999f)
            {
                return cell;
            }
        }
        Debug.Log("ERROR No close cell has been found error!");
        return null;
    }
    public static void CheckIfWon(BasePiece[,] wPA, BasePiece wPiece)
    {
        int count1 = 0;
        int count2 = 0;

        int preVal = 0;
        int numInRow = 1;

        int x = wPiece.x;
        int y = wPiece.y;
        int bs = GameManager.boardSize - 1;

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
            Debug.Log("One to the right!");
        if (x != bs - 1 && x != bs && wPA[x + 2, y] != null && wPA[x + 2, y].team == wPiece.team)
            Debug.Log("One two to the right");
    }
}