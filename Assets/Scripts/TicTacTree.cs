using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacTree
{

    private Node<BoardValue> root;
    public Node<BoardValue> currentNode;
    private bool playerOne;
    private bool yourTurn;

    public TicTacTree(bool pPlayerOne)
    {
        root = new Node<BoardValue>{ Info = new BoardValue() };
        currentNode = root;
        playerOne = pPlayerOne;
        yourTurn = pPlayerOne;
        GenerateTree();
    }

    public void MakeMove(int row, int col)
    {
        Vector2 pos = new Vector2(row, col);
        yourTurn = !yourTurn;
        foreach (Node<BoardValue> child in currentNode.Childs)
        {
            if (child.Info.MoveMade == pos)
            {
                currentNode = child;
                break;
            }
        }
    }

    public Vector2 FindNextMove()
    {
        Node<BoardValue> bestChild;

        if (playerOne)
            bestChild = FindMaxChild(currentNode);
        else
            bestChild = FindMinChild(currentNode);

        return bestChild.Info.MoveMade;
    }

    private Node<BoardValue> FindMinChild(Node<BoardValue> pNode)
    {
        Node<BoardValue> bestChild = new Node<BoardValue> { Info = new BoardValue() };
        bestChild.Info.Value = int.MaxValue;

        foreach (Node<BoardValue> child in pNode.Childs)
        {
            if (child.Info.Value < bestChild.Info.Value)
                bestChild = child;
        }

        return bestChild;
    }

    private Node<BoardValue> FindMaxChild(Node<BoardValue> pNode)
    {
        Node<BoardValue> bestChild = new Node<BoardValue> { Info = new BoardValue() };
        bestChild.Info.Value = int.MinValue;

        foreach (Node<BoardValue> child in pNode.Childs)
        {
            if (child.Info.Value > bestChild.Info.Value)
                bestChild = child;
        }

        return bestChild;
    }

    private void GenerateTree()
    {
        TicTacToe ticTacToe = new TicTacToe();
        Node<BoardValue> currentNode = root;

        GenerateChilds(root, ticTacToe, 0, true);
    }

    private void GenerateChilds(Node<BoardValue> pNode, TicTacToe pTic, int pDepth, bool maximizer)
    {
        int gameState = pTic.IsGameOver();

        if (gameState != -1)
        {
            if (gameState == 1)
                pNode.Info.Value = 10 - pDepth;
            else if (gameState == 2)
                pNode.Info.Value = -10 + pDepth;
            else
                pNode.Info.Value = 0;
        }
        else
        {
            pNode.Info.Value = maximizer ? int.MinValue : int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (pTic.IsValidMove(i, j))
                    {
                        Node<BoardValue> nextNode = new Node<BoardValue>(pNode) { Info = new BoardValue() };
                        nextNode.Info.MoveMade = new Vector2(i, j);
                        pNode.AddChild(nextNode);

                        pTic.MakeMove(i, j);
                        GenerateChilds(nextNode, pTic, pDepth + 1, !maximizer);
                        pTic.UndoMove();

                        if (maximizer)
                            pNode.Info.Value = Mathf.Max(nextNode.Info.Value, pNode.Info.Value);
                        else
                            pNode.Info.Value = Mathf.Min(nextNode.Info.Value, pNode.Info.Value);
                    }
                }
            }
        }
    }
}

public class BoardValue
{
    public int Value { get; set; }
    public Vector2 MoveMade { get; set; }
}