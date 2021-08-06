using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject cCellPrefab;
    public static Cell[,] cellBoard = new Cell[5, 5];

    public void Create()
    {
        int cellCounter = 0;
        for(int y = 0; y < GameManager.boardSize; y++)
        {
            for(int x = 0; x < GameManager.boardSize; x++)
            {
                GameObject newCell = Instantiate(cCellPrefab, new Vector2(x,y), Quaternion.identity) ;

                //cellDictionary.Add(cellCounter, newCell.GetComponent<Cell>());
                cellBoard[x, y] = newCell.GetComponent<Cell>();
                cellBoard[x, y].Setup(new Vector2(x, y), this, cellCounter);
                //cAllCells[x, y].gameObject.GetComponent<SpriteRenderer>().color = Color.; //We stoppen er een witte sprite in en dan veranderen we de kleur.
            }
        }
    }
}
