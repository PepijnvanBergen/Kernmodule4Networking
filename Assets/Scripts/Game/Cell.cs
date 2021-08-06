using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public BasePiece piece = null;
    public int cellInt;

    public Vector2 pos;
    public int x;
    public int y;

    public Board board;

    public void Setup(Vector2 position, Board parentBoard, int cellCounter)
    {
        pos = position;
        transform.position = pos;
        board = parentBoard;
        cellInt = cellCounter;
        x = (int)position.x;
        y = (int)position.y;
    }
}
