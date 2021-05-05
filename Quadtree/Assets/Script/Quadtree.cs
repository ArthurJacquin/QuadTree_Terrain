using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum quadtreeIndex
{
    topleft = 0, // bit 00
    topright = 1, // bit 01
    bottomleft = 2, //bit 10
    bottomright = 3 //bit 11
}


public class Quadtree<TType>
{
    private QuadtreeNode<TType> node;
    //subdivision du cube
    private int depth;

    public Quadtree(Vector2 pos, float s, int d)
    {
        node = new QuadtreeNode<TType>(pos, s);
        depth = d;
    }

    public void Insert(Vector2 pos, TType value )
    {
        node.subdivide(pos, value, depth);
    }

    public static int getIndexPos(Vector2 lookUpPos, Vector2 nodePos)
    {
        int index = 0;

        //si la position est en dessous du pivot du cube alors il est en dessous
        index |= lookUpPos.y < nodePos.y ? 2 : 0;
        index |= lookUpPos.x > nodePos.x ? 1 : 0;


        return index;
    }

    public QuadtreeNode<TType> GetRoot()
    {
        return node;
    }

   
}
