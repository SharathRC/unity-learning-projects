using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrpcTester : MonoBehaviour
{
    // private ColorClient _colorClient;
    private IcpClient _icpClient;
    public Mesh mesh;
    public Vector3[] vertices;
    // public Vector3[] old_world_pts = new Vector3[] {};
    public List<Vector3> old_world_pts = new List<Vector3>();
    // public int[] numArray = new int[] {};  
    // public Vector3[] new_worls_pts;

    // Start is called before the first frame update
    void Start()
    {
        // _colorClient = new ColorClient();
        _icpClient = new IcpClient();
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
            Vector3 worldPt = transform.TransformPoint(vertices[i]);
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
        
    }
}
