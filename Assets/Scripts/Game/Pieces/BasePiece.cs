using UnityEngine;

public class BasePiece : MonoBehaviour
{
    public Cell currentCell;
    public int team;
    public int pieceInt;
    public Vector2 pos;
    public int x;
    public int y;

    public virtual void OnSpawn(Cell spawnCell, int teamInt, int piece, Vector2 position)
    {

    }
    public virtual void OnDeath()
    {

    }
    public virtual void Special()
    {

    }
}
