using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private TicTacToe ticTacToe;
    private TicTacTree botTree;
    private GameObject[] board;
    private Animator[] boardAnim;
    private Button[] boardButton;
    private bool botTurn;

    public Animator[] winLines;
    public Text btnStartText;
    public Button btnNextMove;
    public Toggle cbFirstPlayer;
    public Toggle cbAutoPlay;
    public TreeView treeView;

    public void Start()
    {
        board = GameObject.FindGameObjectsWithTag("Button");
        boardAnim = new Animator[board.Length];
        boardButton = new Button[board.Length];

        for (int i=0; i<board.Length; i++)
        {
            boardAnim[i] = board[i].GetComponent<Animator>();
            boardButton[i] = board[i].GetComponent<Button>();
        }
    }


    public void StartGame()
    {
        ticTacToe = new TicTacToe();
        botTree = new TicTacTree(!cbFirstPlayer.isOn);
        botTurn = !cbFirstPlayer.isOn;
        treeView.StartTreeView(botTree);

        PrepareBoard();

        foreach(Animator line in winLines)
        {
            line.SetBool("Line", false);
        }

        if (botTurn && cbAutoPlay.isOn)
            StartCoroutine(BotMoveWithDelay());

        btnNextMove.interactable = (!cbAutoPlay.isOn && botTurn);
    }

    void PrepareBoard()
    {
        for(int i=0; i<board.Length; i++)
        {
            boardAnim[i].SetBool("Cross", false);
            boardAnim[i].SetBool("Circle", false);
            boardButton[i].enabled = cbFirstPlayer.isOn;
            boardButton[i].interactable = true;
        }
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

        treeView.UpdateTreeView();
    }

    IEnumerator BotMoveWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        BotMove();
        yield return null;
    }

    public void BotMove()
    {
        Vector2 nextMove = botTree.FindNextMove();
        int row = (int)nextMove.x; int col = (int)nextMove.y;
        string btnName = "BtnBoard" + row + col;
        GameObject btnBoard = GameObject.Find(btnName);

        MarkOnBoard(btnBoard);
    }

    public void ChangeTurn()
    {
        foreach(Button btn in boardButton)
        {
            btn.enabled = !btn.enabled;
        }

        botTurn = !botTurn;

        if(botTurn && cbAutoPlay.isOn)
            StartCoroutine(BotMoveWithDelay());

        if(!cbAutoPlay.isOn)
            btnNextMove.interactable = !btnNextMove.interactable;

        if (cbAutoPlay.isOn && btnNextMove.interactable)
            btnNextMove.interactable = false;
    }

    public IEnumerator EndGame(int pWinner)
    {
        foreach (Button btn in boardButton)
        {
            btn.interactable = false;
        }

        int winningLine = (int)ticTacToe.GetWinningLine();

        if (winningLine != -1)
        {
            Animator lineAnim = winLines[winningLine];

            yield return new WaitForSeconds(0.5f);

            lineAnim.SetBool("Line", true);
        }

        yield return null;
    }
}
