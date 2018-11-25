using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeView : MonoBehaviour
{
    public RectTransform treeView;
    public GameObject childPrefab;
    RectTransform parentRect;
    Text parentText;
    Node<BoardValue> currentNode;
    TicTacTree ticTacTree;
    GameObject[] childs = new GameObject[0];

    public void StartTreeView(TicTacTree pTicTacTree)
    {
        ticTacTree = pTicTacTree;
        parentRect = GetComponent<RectTransform>();
        parentText = GetComponentInChildren<Text>();
        UpdateTreeView();
    }

    public void UpdateTreeView()
    {
        foreach(GameObject child in childs)
        {
            Destroy(child);
        }

        currentNode = ticTacTree.currentNode;
        float width = treeView.rect.width;
        float height = treeView.rect.height;
        childs = new GameObject[currentNode.Childs.Count];
        parentText.text = ticTacTree.currentNode.Info.Value.ToString();

        float widthPerChild = width / childs.Length;
        int childSize = (int)widthPerChild;
        float childOffset = (widthPerChild - childSize)/2.0f;
        float distanceOffset = childSize + childOffset;
        float startingPos = treeView.rect.x + childOffset + childSize/2.0f;

        if (childSize > parentRect.rect.width)
            childSize = (int)parentRect.rect.width;

        for (int i = 0; i<childs.Length; i++)
        {
            childs[i] = Instantiate(childPrefab, Vector3.zero, Quaternion.identity, treeView);
            Text text = childs[i].GetComponentInChildren<Text>();
            LineRenderer line = childs[i].GetComponent<LineRenderer>();
            RectTransform childRect = childs[i].GetComponent<RectTransform>();
            Vector3 offset = parentRect.position - childRect.position;

            offset = offset.normalized * childSize / 4f;

            text.text = currentNode.Childs[i].Info.Value.ToString();
            childRect.sizeDelta = Vector2.one * childSize;
            childRect.anchoredPosition = new Vector2(startingPos + distanceOffset*i, -75);
            line.SetPosition(0, childRect.position + Vector3.up*offset.y);
            line.SetPosition(1, parentRect.position - Vector3.up*offset.y - Vector3.forward*90);
        }
    }
}