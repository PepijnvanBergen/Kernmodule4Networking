using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static Dictionary<int, BasePiece> piecesDictionary = new Dictionary<int, BasePiece>();
    public static BasePiece[,] piecesArray = new BasePiece[5, 5];
    public static int piecesInt;
}
