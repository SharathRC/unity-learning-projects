using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrpcTester : MonoBehaviour
{
    // private ColorClient _colorClient;
    private IcpClient _icpClient;
    private Utilities _utilities;
    public Mesh mesh;
    internal Vector3[] main_vertices;
    internal List<Vector3> old_in_camera_pts = new List<Vector3>();

    Camera cam;
    public float cam_speed = 5.0f;
    internal bool camera_moved = false;

    public GameObject object_for_camera;
    internal Mesh object_for_camera_mesh;
    internal Vector3[] object_for_camera_vertices;


    void Start()
    {
        _icpClient = new IcpClient();
        _utilities = new Utilities();
        cam = GetComponent<Camera>();
        mesh = GetComponent<MeshFilter>().mesh;
        main_vertices = mesh.vertices;
        object_for_camera_mesh = object_for_camera.GetComponent<MeshFilter>().mesh;
        // object_for_camera_mesh = object_for_camera.mesh;
    }

    void Update()
    {   
        Matrix4x4 world_T_local = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        Matrix4x4 local_T_world = world_T_local.inverse;

        move_camera_with_arrows();
        Matrix4x4 world_T_camera = _utilities.world2camera();
        Matrix4x4 local_T_camera = local_T_world * world_T_camera;
        Matrix4x4 camera_T_local = local_T_camera.inverse;

        main_vertices = mesh.vertices;
        object_for_camera_vertices = object_for_camera_mesh.vertices;
        var new_in_camera_pts = get_vertices_in_camera(main_vertices, camera_T_local);

        if (old_in_camera_pts.Count == 0)
        {
            old_in_camera_pts = new_in_camera_pts;
        }        

        var camera_transform = _icpClient.getTransform(new_in_camera_pts, old_in_camera_pts);
        // main_vertices = transform_vertices(main_vertices, camera_transform.inverse);

        object_for_camera_vertices = transform_vertices(object_for_camera_vertices, camera_transform.inverse);
        object_for_camera_mesh.vertices = object_for_camera_vertices;

        // mesh.vertices = main_vertices;
        // mesh.RecalculateBounds();

        old_in_camera_pts = new_in_camera_pts;

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

    Vector3[] transform_vertices(Vector3[] vertices_in, Matrix4x4 camera_transform)
    {
        List<Vector3> new_local_pts = new List<Vector3>();
        for (var i = 0; i < vertices_in.Length; i++)
        {
            if (camera_moved)
            {
                Vector3 new_local_pt = camera_transform.MultiplyPoint3x4(vertices_in[i]);
                vertices_in[i] = new_local_pt;
                
            }
        }
        return vertices_in;
    }


    List<Vector3> get_vertices_in_camera(Vector3[] vertices_in, Matrix4x4 camera_T_local)
    {
        List<Vector3> new_in_camera_pts = new List<Vector3>();
        for (var i = 0; i < vertices_in.Length; i++)
        {
            Vector3 camera_pt = camera_T_local.MultiplyPoint3x4(vertices_in[i]);
            new_in_camera_pts.Add(camera_pt);
        }

        return new_in_camera_pts;
    }

}
