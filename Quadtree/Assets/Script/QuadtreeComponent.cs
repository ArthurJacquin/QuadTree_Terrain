using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeComponent : MonoBehaviour
{
    [SerializeField] private float size = 5;
    [SerializeField] private int depth = 2;

    [SerializeField] private Transform[] points = new Transform[0];
    [SerializeField] private Color minColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color maxColor = new Color(1, 0, 0, 0.25f);

    private void OnDrawGizmos()
    {
        Quadtree<bool> quadtree = new Quadtree<bool>(this.transform.position, size, depth);
        foreach(var point in points)
        {
            quadtree.Insert(point.position, true);
        }

        DrawNode(quadtree.GetRoot());
    }


    private void DrawNode(QuadtreeNode<bool> node, int nodeDepth = 0)
    {
        if(node.isLeaf())
        {
            Gizmos.color = Color.magenta;
        }
        else
        {
            foreach(var subnode in node.Nodes)
            {
                if(subnode != null) 
                    DrawNode(subnode, nodeDepth + 1);
            }
        }

        Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
        Gizmos.DrawWireCube(node.position, new Vector3(1, 1, 0.1f) * node.size);
    }


}
