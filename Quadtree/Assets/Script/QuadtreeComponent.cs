using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeComponent : MonoBehaviour
{
    [SerializeField] private float size = 5;
    [SerializeField] private int depth = 2;
    [SerializeField] private float radius = 0.1f;

    [SerializeField] private Transform[] points = new Transform[8];
    [SerializeField] private Color minColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color maxColor = new Color(1, 0, 0, 0.25f);

    [SerializeField] List<GameObject> poolList;
    private Queue<GameObject> pool;
    
    private Quadtree<bool> quadtree;
    private Vector3 lastPlayerPos;

    private void Start()
    {
        pool = new Queue<GameObject>(poolList);
        quadtree = new Quadtree<bool>(this.transform.position, size, depth);
        lastPlayerPos = Vector3.negativeInfinity;
    }

    private void Update()
    {
        if (lastPlayerPos == points[0].position)
            return;

        if (pool == null || pool.Count == 0)
            return;

        ClearQuadTree();
        lastPlayerPos = points[0].position;
        quadtree = new Quadtree<bool>(this.transform.position, size, depth);
        foreach (var point in points)
        {
            quadtree.Insert(point.position, true);
        }

        CreateQuads(quadtree.GetRoot());
    }

    private void OnDrawGizmos()
    {
        float offsetAngle = (360.0f / (points.Length - 1.0f)) * Mathf.Deg2Rad;
        for (int i = 1; i < points.Length; i++)
        {
            points[i].localPosition = new Vector3(Mathf.Cos((i - 1) * offsetAngle), Mathf.Sin((i - 1) * offsetAngle), 0) * radius;
        }

        if(quadtree == null)
        {
            quadtree = new Quadtree<bool>(this.transform.position, size, depth);
            foreach (var point in points)
            {
                quadtree.Insert(point.position, true);
            }
        }

        DrawNode(quadtree.GetRoot());
    }


    private void DrawNode(QuadtreeNode<bool> node, int nodeDepth = 0)
    {
        if(node.isLeaf())
        {
            Gizmos.color = Color.magenta;
            Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
            Gizmos.DrawWireCube(node.position, new Vector3(1, 1, 0.1f) * node.size);
        }
        else
        {
            foreach(var subnode in node.Nodes)
            {
                if(subnode != null) 
                    DrawNode(subnode, nodeDepth + 1);
            }
        }
    }

    private void CreateQuads(QuadtreeNode<bool> node, int nodeDepth = 0)
    {
        if (node.isLeaf())
        {
            GameObject go = pool.Dequeue();

            go.transform.localPosition = node.position - Vector2.one * node.size / 2;
            go.transform.localScale = Vector3.one * node.size / 4;

            go.SetActive(true);
        }
        else
        {
            foreach (var subnode in node.Nodes)
            {
                if (subnode != null)
                    CreateQuads(subnode, nodeDepth + 1);
            }
        }
    }

    private void ClearQuadTree()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                pool.Enqueue(transform.GetChild(i).gameObject);
            }
        }
    }
}
