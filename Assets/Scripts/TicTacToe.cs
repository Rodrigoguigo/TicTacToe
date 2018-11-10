using System.Collections.Generic;
using UnityEngine;

public class TicTacToe
{
    private int[,] board;
    private int movesLeft;
    private bool cross;
    private Stack<Vector2> movesMade;
    public Vector2[] rowPoints;
    public Vector2[] colPoints;
    public Vector2 diag;
    public Vector2 antiDiag;

    public TicTacToe()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        board = new int[3, 3];
        movesLeft = 9;
        cross = true;
        movesMade = new Stack<Vector2>();
        rowPoints = new Vector2[3];
        colPoints = new Vector2[3];
        diag = new Vector2();
        antiDiag = new Vector2();
    }

    public void MakeMove(int row, int col)
    {
        if (IsValidMove(row, col))
        {
            Vector2 pointPlace = cross ? Vector2.right : Vector2.up;
            board[row, col] = cross ? 1 : 2;
            movesLeft--;
            cross = !cross;

            rowPoints[row] += pointPlace;
            colPoints[col] += pointPlace;

            if (row == col)
                diag += pointPlace;

            if (IsAntiDiagonal(row, col))
                antiDiag += pointPlace;

            movesMade.Push(new Vector2(row, col));
        }
    }

    public void UndoMove()
    {
        if(movesMade.Count > 0)
        {
            Vector2 pos = movesMade.Pop();
            int row = (int)pos.x; int col = (int)pos.y;
            Vector2 pointPlace = cross ? Vector2.up : Vector2.right;
            board[row, col] = 0;
            movesLeft++;
            cross = !cross;

            rowPoints[row] -= pointPlace;
            colPoints[col] -= pointPlace;

            if (row == col)
                diag -= pointPlace;

            if (IsAntiDiagonal(row, col))
                antiDiag -= pointPlace;
        }
    }

    public bool IsValidMove(int row, int col)
    {
        return board[row, col] == 0;
    }

    private bool IsAntiDiagonal(int row, int col)
    {
        int diff = Mathf.Abs(row - col);

        return diff == 2 || (diff == 0 && row == 1);
    }

    public int IsGameOver()
    {
        for(int i=0; i<3; i++)
        {
            if (rowPoints[i].x == 3 || colPoints[i].x == 3)
                return 1;
            else if (rowPoints[i].y == 3 || colPoints[i].y == 3)
                return 2;
        }

        if (diag.x == 3 || antiDiag.x == 3)
            return 1;
        else if (diag.y == 3 || antiDiag.y == 3)
            return 2;

        if (movesLeft == 0)
            return 0;

        return -1;
    }

    public int IsGameOver(int row, int col)
    {
        if (rowPoints[row].x == 3 || colPoints[col].x == 3)
            return 1;
        else if (rowPoints[row].y == 3 || colPoints[col].y == 3)
            return 2;

        if (diag.x == 3 || antiDiag.x == 3)
            return 1;
        else if (diag.y == 3 || antiDiag.y == 3)
            return 2;

        if (movesLeft == 0)
            return 0;

        return -1;
    }

    public WinningLine GetWinningLine()
    {
        for(int i=0; i<3; i++)
        {
            if (rowPoints[i].magnitude == 3)
                return (WinningLine) i ;
            else if (colPoints[i].magnitude == 3)
                return (WinningLine)(i + 3);
        }

        if (diag.magnitude == 3)
            return (WinningLine) 6;
        else if (antiDiag.magnitude == 3)
            return (WinningLine) 7;

        return (WinningLine)(-1);
    }

    public bool GetTurn()
    {
        return cross;
    }

    public enum WinningLine
    {
        NoWinner = -1,
        FirstRow = 0,
        SecondRow = 1,
        ThirdRow = 2,
        FirstColumn = 3,
        SecondColumn = 4,
        ThirdColumn = 5,
        Diagonal = 6,
        AntiDiagonal = 7
    };
}
