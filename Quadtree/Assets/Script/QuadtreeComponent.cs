using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeComponent : MonoBehaviour
{
    [SerializeField] private float size = 5;
    [SerializeField] private int depth = 2;


    private void OnDrawGizmos()
    {
        Quadtree<int> quadtree = new Quadtree<int>(this.transform.position, size, depth);

        DrawNode(quadtree.GetRoot());
    }

    private void DrawNode(QuadtreeNode<int> node)
    {
        if(node.isLeaf())
        {
            Gizmos.color = Color.magenta;
        }
        else
        {
            Gizmos.color = Color.blue;
            foreach(var subnode in node.Nodes)
            {
                DrawNode(subnode);
            }
        }

        Gizmos.DrawWireCube(node.position, Vector2.one * node.size);
    }


}
