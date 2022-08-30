using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Draw2DShapesLite;

public class ButtonHandler : MonoBehaviour
{

    public GameObject manager;
    private ConvexHull convex;
    private PointsManager pManager;
    public GameObject polygonDrawer;
    

    // Start is called before the first frame update
    void Start()
    {
        convex = manager.GetComponent<ConvexHull>();
        pManager = manager.GetComponent<PointsManager>();
        
    }

   public void CalculateConvex()
    {
        convex.ComputeConvex(pManager.nodes);
        convex.CreateEdges(pManager.nodes);
        convex.DrawArestas();
    }

    public void CreatePolygon()
    {
        Draw2D d = polygonDrawer.GetComponent<Draw2D>();
        convex = manager.GetComponent<ConvexHull>();
        Stack<Node> hull = convex.convexHull;
        List<Vector3> points = new List<Vector3>();
        Vector3 temp;
        Node tempNode;
        int n = hull.Count;

        for (int i = 0; i < n; i++)
        {
            tempNode = hull.Pop();
            temp.x = tempNode.x;
            temp.y = tempNode.y;
            temp.z = 0f;
            points.Add(temp);
        }
        d.vertices = points;
        d.MakeMesh();
        
    }
    public void Update()
    {
        
    }
}
