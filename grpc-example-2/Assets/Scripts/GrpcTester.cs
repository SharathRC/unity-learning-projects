using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrpcTester : MonoBehaviour
{
    // private ColorClient _colorClient;
    private IcpClient _icpClient;
    private Utilities _utilities;
    public Mesh mesh;
    public Vector3[] vertices;
    public List<Vector3> old_world_pts = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // _colorClient = new ColorClient();
        _icpClient = new IcpClient();
        _utilities = new Utilities();
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {   
        List<Vector3> new_world_pts = new List<Vector3>();
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] += Vector3.up * Time.deltaTime;
            // print("vertice: " + vertices[i]);
            Vector3 worldPt = transform.TransformPoint(vertices[i]);
            // print("worldpoint: " + worldPt);
            new_world_pts.Add(worldPt);
        }

        if (old_world_pts.Count == 0)
        {
            old_world_pts = new_world_pts;
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        var res = _icpClient.getTransform(new_world_pts, old_world_pts);

        // var newColorString = _colorClient.GetRandomColor("oldcolor");
        // print(newColorString);
        // print(res);
        old_world_pts = new_world_pts;
        _utilities.camera2world();
    }
}
