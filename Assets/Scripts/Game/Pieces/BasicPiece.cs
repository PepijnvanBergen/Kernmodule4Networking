using UnityEngine;

public class BasicPiece : BasePiece
{
    public override void OnSpawn(Cell spawnCell, int teamInt, int piece, Vector2 position)
    {
        currentCell = spawnCell;
        spawnCell.piece = this;
        team = teamInt;
        pieceInt = piece;
        pos = position;
        x = (int)pos.x;
        y = (int)pos.y;
    }
    public override void OnDeath()
    {
        currentCell.piece = null;
    }
}