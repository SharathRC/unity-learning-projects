using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrpcTester : MonoBehaviour
{
    // private ColorClient _colorClient;
    private IcpClient _icpClient;
    private Utilities _utilities;
    public Mesh mesh;
    internal Vector3[] vertices;
    internal List<Vector3> old_in_camera_pts = new List<Vector3>();
    internal Matrix4x4 world_T_camera;
    Camera cam;
    public float cam_speed = 5.0f;
    internal bool camera_moved = false;
    internal Matrix4x4 world_T_local;
    internal Matrix4x4 local_T_camera;
    internal Matrix4x4 camera_T_local;

    void Start()
    {
        _icpClient = new IcpClient();
        _utilities = new Utilities();
        cam = GetComponent<Camera>();
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void Update()
    {   
        world_T_local = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        Matrix4x4 local_T_world = world_T_local.inverse;

        move_camera_with_arrows();
        world_T_camera = _utilities.world2camera();
        local_T_camera = local_T_world * world_T_camera;
        camera_T_local = local_T_camera.inverse;

        vertices = mesh.vertices;
        var new_in_camera_pts = get_vertices_in_camera();
        var vertices_list = _utilities.vector3_to_list(vertices);
        // _utilities.print_vertices(vertices_list);
        // _utilities.print_vertices(new_in_camera_pts);

        if (old_in_camera_pts.Count == 0)
        {
            old_in_camera_pts = new_in_camera_pts;
        }        

        var camera_transform = _icpClient.getTransform(new_in_camera_pts, old_in_camera_pts);
        // print(camera_transform);
        transform_vertices(camera_transform.inverse);
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        old_in_camera_pts = new_in_camera_pts;

        // world_T_camera = _utilities.world2camera();
    }

    void move_camera_with_arrows()
    {   
        camera_moved = false;
        if(Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.Translate(new Vector3(cam_speed * Time.deltaTime,0,0));
            camera_moved = true;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.Translate(new Vector3(-cam_speed * Time.deltaTime,0,0));
            camera_moved = true;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.Translate(new Vector3(0,-cam_speed * Time.deltaTime,0));
            camera_moved = true;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.Translate(new Vector3(0,cam_speed * Time.deltaTime,0));
            camera_moved = true;
        }
    }

    void transform_vertices(Matrix4x4 camera_transform)
    {
        List<Vector3> new_local_pts = new List<Vector3>();
        for (var i = 0; i < vertices.Length; i++)
        {
            // vertices[i] += Vector3.up * Time.deltaTime;
            // Vector3 world_pt = transform.TransformPoint(vertices[i]);

            // print("wpt" + world_pt);
            // print(world_T_local.MultiplyPoint3x4(vertices[i]));

            if (camera_moved)
            {
                Vector3 new_local_pt = camera_transform.MultiplyPoint3x4(vertices[i]);
                vertices[i] = new_local_pt;
                
            }

            // print("wpt" + world_pt);
            // print("wtc" + world_T_camera);
            // print("tvc" + vertices[i]);

            // new_local_pts.Add(new_local_pt);
        }
    }


    List<Vector3> get_vertices_in_camera()
    {
        List<Vector3> new_in_camera_pts = new List<Vector3>();
        for (var i = 0; i < vertices.Length; i++)
        {
            Vector3 camera_pt = camera_T_local.MultiplyPoint3x4(vertices[i]);
            new_in_camera_pts.Add(camera_pt);
        }

        return new_in_camera_pts;
    }

}
