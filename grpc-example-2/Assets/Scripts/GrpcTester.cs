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
    internal List<Vector3> old_world_pts = new List<Vector3>();
    internal Matrix4x4 world_T_camera;
    Camera cam;
    public float cam_speed = 5.0f;
    internal bool camera_moved = false;
    internal Matrix4x4 world_T_local;
    internal Matrix4x4 local_T_camera;

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
        local_T_camera = local_T_world * world_T_camera;

        move_camera_with_arrows();
        world_T_camera = _utilities.world2camera();

        vertices = mesh.vertices;
        var new_world_pts = get_mesh_world_points();

        if (old_world_pts.Count == 0)
        {
            old_world_pts = new_world_pts;
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        var camera_transform = _icpClient.getTransform(new_world_pts, old_world_pts);

        old_world_pts = new_world_pts;
        world_T_camera = _utilities.world2camera();
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

    List<Vector3> get_mesh_world_points()
    {
        List<Vector3> new_world_pts = new List<Vector3>();
        for (var i = 0; i < vertices.Length; i++)
        {
            // vertices[i] += Vector3.up * Time.deltaTime;
            Vector3 world_pt = transform.TransformPoint(vertices[i]);

            

            if (camera_moved)
            {
                Vector3 camera_pt = local_T_camera.MultiplyPoint3x4(vertices[i]);

                Matrix4x4 camera_T_world = world_T_camera.inverse;
                Vector3 new_world_pt = camera_T_world.MultiplyPoint3x4(camera_pt);
            
                Vector3 local_pt = transform.InverseTransformPoint(new_world_pt);
                vertices[i] = local_pt;
                
            }

            print("wpt" + world_pt);
            print("wtc" + world_T_camera);
            print("tvc" + vertices[i]);

            new_world_pts.Add(world_pt);
        }
        return new_world_pts;
    }

}
