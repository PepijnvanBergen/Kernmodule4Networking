using UnityEngine;
public class ChooseTurn : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    public enum playerTurn
    {
        player1 = 0,
        player2 = 1
    }
    private playerTurn pTurn;

    void Update()
    {
        //ChooseInputs();
        //ChangeTurn();
    }
    private void ChooseInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (pTurn == playerTurn.player1)
            {
                Actions.PlacePiece(Actions.FindMouseCell(FindMouse(cam)), 1, 1);
            }
            else if (pTurn == playerTurn.player2)
            {
                Actions.PlacePiece(Actions.FindMouseCell(FindMouse(cam)), 2, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (pTurn == playerTurn.player1)
            {
                Actions.PlacePiece(Actions.FindMouseCell(FindMouse(cam)), 1, 2);
            }
            else if (pTurn == playerTurn.player2)
            {
                Actions.PlacePiece(Actions.FindMouseCell(FindMouse(cam)), 2, 2);
            }
        }
    }
    public  void ChangeTurn()
    {
        if (pTurn == playerTurn.player1 && Actions.hasDoneAction == true)
        {
            pTurn = playerTurn.player2;
            Actions.hasDoneAction = false;
        }
        else if (pTurn == playerTurn.player2 && Actions.hasDoneAction == true)
        {
            pTurn = playerTurn.player1;
            Actions.hasDoneAction = false;
        }
    }
    private Vector2 FindMouse(Camera cam)
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        return mouseWorldPos;
    }
}