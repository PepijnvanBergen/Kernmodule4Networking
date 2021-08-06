using UnityEngine;
using System.Collections.Generic;
public class WallPiece : BasePiece
{
    [SerializeField]
    private GameObject blockerPiece;
    public override void OnSpawn(Cell spawnCell, int teamInt, int piece, Vector2 position)
    {
        currentCell = spawnCell;
        spawnCell.piece = this;
        team = teamInt;
        pieceInt = piece;
        pos = position;
        x = (int)pos.x;
        y = (int)pos.y;
        Wall(currentCell);
    }
    public override void OnDeath()
    {
        currentCell.piece = null;
    }
    public void Wall(Cell wCell)
    {
        Vector2 wCellPos = wCell.pos;
        List<Cell> usableCells = new List<Cell>();

        Debug.Log(wCellPos.y);
        Debug.Log(GameManager.boardSize);

        if (wCellPos.x > 0 && Board.cellBoard[(int)wCellPos.x - 1, (int)wCellPos.y] != null)
        {
            usableCells.Add(Board.cellBoard[(int)wCellPos.x - 1, (int)wCellPos.y]);
        }
        if(wCellPos.x + 1 < GameManager.boardSize && Board.cellBoard[(int)wCellPos.x + 1, (int)wCellPos.y] != null)
        {
            usableCells.Add(Board.cellBoard[(int)wCellPos.x + 1, (int)wCellPos.y]);
        }
        if (wCellPos.y > 0 && Board.cellBoard[(int)wCellPos.x, (int)wCellPos.y - 1] != null)
        {
            usableCells.Add(Board.cellBoard[(int)wCellPos.x, (int)wCellPos.y - 1]);
        }
        if (wCellPos.y + 1 < GameManager.boardSize && Board.cellBoard[(int)wCellPos.x, (int)wCellPos.y + 1] != null)
        {
            usableCells.Add(Board.cellBoard[(int)wCellPos.x, (int)wCellPos.y + 1]);
        }
        Debug.Log(usableCells.Count);
        foreach (Cell usableCell in usableCells)
        {
            GameObject piece = Instantiate(blockerPiece, usableCell.pos, Quaternion.identity);
            usableCell.piece = piece.GetComponent<BasePiece>();
        }
    }
}