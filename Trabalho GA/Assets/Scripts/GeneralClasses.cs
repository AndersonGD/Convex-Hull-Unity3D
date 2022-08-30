    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Node
{
    public int id;
    public float x;
    public float y;
    public Node(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public Node(float x, float y, int id)
    {
        this.x = x;
        this.y = y;
        this.id = id;
    }
}

[System.Serializable]
public class Aresta
{
    //public bool isChecked = false;
    public float value;
    public Node[] nodes = new Node[2];
    public Aresta(Node n1, Node n2, float v)
    {
        nodes[0] = n1;
        nodes[1] = n2;
        value = v;
    }

    //public static double getLength(Node node1, Node node2)
    //{
    //   
    //    double length;
    //    length = Math.Pow(node1.y - node2.y, 2) + Math.Pow(node1.x - node2.x, 2);
    //    //length = Math.sqrt(Math.Pow(node1.y - node2.y, 2) + Math.Pow(node1.x - node2.x, 2));
    //    return length;
    //}


}