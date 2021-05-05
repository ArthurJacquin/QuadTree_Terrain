using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode<TType>
{
    Vector2 nodePosition;
    float nodeSize; //cube

    QuadtreeNode<TType>[] subNodes;
    IList<TType> value;

    public QuadtreeNode(Vector2 pos, float s)
    {
        nodePosition = pos;
        nodeSize = s;
    }

    public IEnumerable<QuadtreeNode<TType>> Nodes
    {
        get { return subNodes; }
    }

    public float size
    {
        get { return nodeSize; }
    }

    public Vector2 position
    {
        get { return nodePosition; }
    }

    public void subdivide(int depth = 0)
    {
        subNodes = new QuadtreeNode<TType>[4];
        for (int i = 0; i < subNodes.Length; ++i)
        {
            Vector2 newPos = nodePosition;
            if ((i & 2) == 2)
            {
                newPos.y -= nodeSize * 0.25f;
            }
            else
            {
                newPos.y += nodeSize * 0.25f;
            }

            if(( i & 1) == 1)
            {
                newPos.x += nodeSize * 0.25f;
            }
            else
            {
                newPos.x -= nodeSize * 0.25f;
            }
            subNodes[i] = new QuadtreeNode<TType>(newPos, nodeSize * 0.5f);
            if(depth > 0)
            {
                subNodes[i].subdivide(depth - 1);
            }
        }
    }

    public bool isLeaf()
    {
        return subNodes == null;
    }
}
