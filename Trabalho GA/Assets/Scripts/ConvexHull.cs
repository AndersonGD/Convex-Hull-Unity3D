using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ConvexHull : MonoBehaviour{

    [SerializeField]
    public Stack<Node> convexHull = new Stack<Node>();
    public List<Aresta> arestas = new List<Aresta>();
    public Material material;


Node nextToTop(ref Stack<Node> S)
{
    Node p = S.Peek();
    S.Pop();
    Node res = S.Peek();
    S.Push(p);
    return res;
}


public void SwapNodes(ref Node p1, ref Node p2)
{
    Node temp = p1;
    p1 = p2;
    p2 = temp;
}


float distSq(Node p1, Node p2)
{
    return (p1.x - p2.x) * (p1.x - p2.x) +
        (p1.y - p2.y) * (p1.y - p2.y);
}

   
    float orientation(Node p, Node q, Node r)
    {
        float val = (q.y - p.y) * (r.x - q.x) -
                (q.x - p.x) * (r.y - q.y);

        if (val == 0) return 0;
        return (val > 0) ? 1 : 2;
    }

    public static double getAngle(Node p1, Node p2)
    {
        double xDiff = p2.x - p1.x;
        double yDiff = p2.y - p1.y;
        return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
    }

    public static List<Node> MergeSort(Node p0, List<Node> arrPoint)
    {
        if (arrPoint.Count == 1)
        {
            return arrPoint;
        }
        List<Node> arrSortedInt = new List<Node>();
        int middle = (int)arrPoint.Count / 2;
        List<Node> leftArray = arrPoint.GetRange(0, middle);
        List<Node> rightArray = arrPoint.GetRange(middle, arrPoint.Count - middle);
        leftArray = MergeSort(p0, leftArray);
        rightArray = MergeSort(p0, rightArray);
        int leftptr = 0;
        int rightptr = 0;
        for (int i = 0; i < leftArray.Count + rightArray.Count; i++)
        {
            if (leftptr == leftArray.Count)
            {
                arrSortedInt.Add(rightArray[rightptr]);
                rightptr++;
            }
            else if (rightptr == rightArray.Count)
            {
                arrSortedInt.Add(leftArray[leftptr]);
                leftptr++;
            }
            else if (getAngle(p0, leftArray[leftptr]) < getAngle(p0, rightArray[rightptr]))
            {
                arrSortedInt.Add(leftArray[leftptr]);
                leftptr++;
            }
            else
            {
                arrSortedInt.Add(rightArray[rightptr]);
                rightptr++;
            }
        }
        return arrSortedInt;
    }

 
    Stack<Node> DoConvex(List<Node> nodes, int n)
{

    float ymin = nodes[0].y;
    int min = 0;
    for (int i = 1; i < n; i++)
    {
        float y = nodes[i].y;

       
        if ((y < ymin) || (ymin == y &&
            nodes[i].x < nodes[min].x))
        {
            ymin = nodes[i].y;
            min = i;
        }
    }
        Node t1 = nodes[0];
        Node t2 = nodes[min];
   
        nodes[0] = t2;
        nodes[min] = t1;
        
        Node p0;
        p0 = nodes[0];
        nodes = MergeSort(p0, nodes);

   
    int m = 1;
    for (int i = 1; i < n; i++)
    {
       
        while (i < n - 1 && orientation(p0, nodes[i],
                                        nodes[i + 1]) == 0)
            i++;


            nodes[m] = nodes[i];
        m++; 
    }

   
    if (m < 3) return null;

    
    Stack<Node> S = new Stack<Node>();
    S.Push(nodes[0]);
    S.Push(nodes[1]);
    S.Push(nodes[2]);

   
    for (int i = 3; i < m; i++)
    {
        
        while (orientation(nextToTop(ref S), S.Peek(), nodes[i]) != 2)
            S.Pop();
        S.Push(nodes[i]);
    }

  
        return S;
}

    public void CreateEdges(List<Node> nodes)
    {
        //float dist = Math.Sqrt(distSq());
        Dictionary<int, float> distances = new Dictionary<int, float>();
        Node p1 = new Node(0,0,0);
        Node p2 = new Node(0,0,0);
        int n = nodes.Count;
        List<int> nodesIDs = new List<int>();
        float dist = 0;
        float minDist = 9999999;
        for (int i = 0; i < n; i++)
        {
            p1 = nodes[i];
            for (int j = 0; j < n; j++)
            {
                p2 = nodes[j];
                if (p1.id != p2.id){
                    dist = distSq(p1, p2);
                    distances.Add(p2.id, dist);
                    if(dist < minDist)
                    {
                        minDist = dist;
                    }
                }
            }
            

            //sort distances by value
            foreach (KeyValuePair<int, float> id in distances.OrderBy(key => key.Value))
            {
                nodesIDs.Add(id.Key);
            }
            //distances.Sort();
            int nn = arestas.Count;
            bool added = false;
            int counterIds = 0;

            for (int k = 0; k < nn; k++)
            {
                if (arestas[k].nodes[0].id == nodesIDs[counterIds] && arestas[k].nodes[1].id == p1.id)
                {
                    //proximo mais perto
                    if (counterIds < nn)
                    {
                        counterIds++;
                        k = 0;
                        added = true;
                    }
                }
                else
                {
                    added = false;
                }
            }
            if (!added)
            {
                //create aresta
                for (int y = 0; y < n; y++)
                {
                    if(nodesIDs[counterIds] == nodes[y].id)
                    {
                        p2 = nodes[y];
                        break;
                    }
                }
                float dd = 0f;
                distances.TryGetValue(nodesIDs[counterIds], out dd);
                Aresta aresta = new Aresta(p1,p2, dd);
                arestas.Add(aresta);
            }

            distances.Clear();
            nodesIDs.Clear();
            minDist = 9999999;
        }
    }

    public void DrawArestas()
    {
        Vector3 start = new Vector3(0,0,0);
        Vector3 end = new Vector3(0, 0, 0);
        for (int i = 0; i < arestas.Count; i++)
        {
            start.x = arestas[i].nodes[0].x;
            start.y = arestas[i].nodes[0].y;
            start.z = -1f;
            end.x = arestas[i].nodes[1].x;
            end.y = arestas[i].nodes[1].y;
            end.z = -1f;
            DrawLine(start, end);
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.material = material;
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
    }

   
    public void ComputeConvex(List<Node> nodes)
{

        int n = nodes.Count;
        convexHull = DoConvex(nodes, n);
}

}