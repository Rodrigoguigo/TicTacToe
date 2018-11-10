using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private TicTacToe ticTacToe;
    private TicTacTree botTree;
    private GameObject[] boardButtons;
    private bool botTurn;

    public GameObject[] winLines;

    // Use this for initialization
    void Start()
    {
        ticTacToe = new TicTacToe();
        botTree = new TicTacTree(false);
        botTurn = false;
        boardButtons = GameObject.FindGameObjectsWithTag("Button");
    }

    public void MarkOnBoard(GameObject pBtnBoard)
    {
        Animator anim = pBtnBoard.GetComponent<Animator>();
        int row = (int)char.GetNumericValue(pBtnBoard.name[8]);
        int col = (int)char.GetNumericValue(pBtnBoard.name[9]);
        bool isCross = ticTacToe.GetTurn();

        if (ticTacToe.IsValidMove(row, col))
        {
            ticTacToe.MakeMove(row, col);
            botTree.MakeMove(row, col);

            if (isCross)
                anim.SetBool("Cross", true);
            else
                anim.SetBool("Circle", true);

            int win = ticTacToe.IsGameOver(row, col);

            if (win != -1)
                StartCoroutine(EndGame(win));
            else
                ChangeTurn();
        }
    }

    public IEnumerator BotMove()
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 nextMove = botTree.FindNextMove();
        int row = (int)nextMove.x; int col = (int)nextMove.y;
        string btnName = "BtnBoard" + row + col;
        GameObject btnBoard = GameObject.Find(btnName);

        MarkOnBoard(btnBoard);
        yield return null;
    }

    public void ChangeTurn()
    {
        foreach(GameObject btn in boardButtons)
        {
            Button btnButton = btn.GetComponent<Button>();
            btnButton.enabled = !btnButton.enabled;
        }

        botTurn = !botTurn;

        if (botTurn)
            StartCoroutine(BotMove());
    }

    public IEnumerator EndGame(int pWinner)
    {
        foreach (GameObject btn in boardButtons)
        {
            btn.GetComponent<Button>().interactable = false;
        }

        int winningLine = (int)ticTacToe.GetWinningLine();
        Animator lineAnim = winLines[winningLine].GetComponent<Animator>();

        yield return new WaitForSeconds(0.5f);

        lineAnim.SetBool("Line", true);

        yield return null;
    }
}
