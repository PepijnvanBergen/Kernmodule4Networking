using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Player A is positive, Player B negative
/// </summary>
[ExecuteInEditMode]
public class TicTacToe : MonoBehaviour
{
    public int GRID_SIZE = 5;
    public int[,] board;

    void Start()
    {
        board = new int[GRID_SIZE, GRID_SIZE];
    }

    public void WonPosition(int x, int y, int value)
    {

        // line right
        if (x + 2 < board.GetLength(0)
            && board[x + 1, y] + board[x + 2, y] + value == 3)
            Debug.Log("Player A wins");
        if (x + 2 < board.GetLength(0)
            && board[x + 1, y] + board[x + 2, y] + value == -3)
            Debug.Log("Player B wins");

        //In the middle
        if(x + 1 < board.GetLength(0) && x - 1 > 0
            && board[x + 1, y] + board[x -1, y] + value == 3)
        if (x + 1 < board.GetLength(0) && x - 1 > 0
            && board[x + 1, y] + board[x - 1, y] + value == -3)

        // line left
        if (x - 2 > 0
            && board[x - 1, y] + board[x - 2, y] + value == 3)
            Debug.Log("Player A wins");
        if (x - 2 > 0
            && board[x - 1, y] + board[x - 2, y] + value == -3)
            Debug.Log("Player B wins");

        // Diagonal ascending right
        if (x + 2 < board.GetLength(0) && y + 2 < board.GetLength(1)
            && board[x + 1, y + 1] + board[x + 2, y + 2] + value == 3)
            Debug.Log("Player A wins");
        if (x + 2 < board.GetLength(0) && y + 2 < board.GetLength(1)
           && board[x + 1, y + 1] + board[x + 2, y + 2] + value == -3)
            Debug.Log("Player B wins");

        // and so on
    }


#if UNITY_EDITOR
    private void OnEnable() => Start();
#endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(TicTacToe))]
public class TicTacToeEditor : Editor
{
    TicTacToe ttt;
    int testPosX, testPosY;
    bool playerA = true;
    public override void OnInspectorGUI()
    {
        ttt = (TicTacToe)target;
        for (int y = 0; y < ttt.GRID_SIZE; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < ttt.GRID_SIZE; x++)
            {
                ttt.board[x, y] = EditorGUILayout.IntField(ttt.board[x, y]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        testPosX = EditorGUILayout.IntField(testPosX);
        testPosY = EditorGUILayout.IntField(testPosY);
        playerA = EditorGUILayout.Toggle(playerA);
        if (GUILayout.Button("Test Position"))
        {
            ttt.WonPosition(testPosX, testPosY, playerA ? +1 : -1);
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif
