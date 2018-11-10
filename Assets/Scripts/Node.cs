using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    private readonly List<Node<T>> childs;
    private readonly Node<T> parent;

    public T Info { get; set; }
    public List<Node<T>> Childs
    {
        get
        {
            return childs;
        }
    }

    public Node<T> GetParent()
    {
        return parent;
    }

    public Node()
    {
        childs = new List<Node<T>>();
    }

    public Node(Node<T> pParent)
    {
        parent = pParent;
        childs = new List<Node<T>>();
    }


    public void AddChild(Node<T> pNode)
    {
        Childs.Add(pNode);
    }
}
