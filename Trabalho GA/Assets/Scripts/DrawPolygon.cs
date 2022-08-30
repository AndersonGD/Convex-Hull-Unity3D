using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DrawPolygon : MonoBehaviour
{
    private Mesh mesh;
    public GameObject manager;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

    }

    // Update is called once per frame
    void Update()
    {
        Graphics.DrawMesh(mesh, new Vector3(0, 0, 10), Quaternion.identity, material, 0);
    }

    public void DrawP()
    {
        ConvexHull convex;
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
            temp.z = 10f;
            points.Add(temp);
        }
        


        mesh.SetVertices(points);

        //mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
