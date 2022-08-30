using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public List<Node> nodes = new List<Node>();
    public GameObject graphic;
    private int counter = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec3 = Input.mousePosition;
            vec3.z = 10.0f;
            vec3 = Camera.main.ScreenToWorldPoint(vec3);
           
            if (vec3.x <= 3.3f && vec3.x > -6.6f)
            {
                Node t;
                t = new Node(vec3.x, vec3.y);
                t.id = counter;
                nodes.Add(t);
                counter++;
                GameObject.Instantiate(graphic, vec3, Quaternion.identity);
            }
        }
    }

public RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
{
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    Physics.Raycast(ray, out hit, rayLength);
    return hit;
}

}
