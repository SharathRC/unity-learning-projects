using Grpc.Core;
using Protoicp;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IcpClient : MonoBehaviour
{
    private readonly CalcTransform.CalcTransformClient _client;
    private readonly Channel _channel;
    // private readonly string _server = "127.0.0.1:50051";
    private readonly string _server = "localhost:50051";

    internal IcpClient() {
        _channel = new Channel(_server, ChannelCredentials.Insecure);
        _client = new CalcTransform.CalcTransformClient(_channel);
    }


    Cloud vector2cloud(List<Vector3> vector_pts )
    {   
        Cloud cloud = new Cloud
        {
            Points = {}
        };

        for(int i=0; i<vector_pts.Count; i++)
        {   
            float x = vector_pts[i].x;
            float y = vector_pts[i].y;
            float z = vector_pts[i].z;
            Point p = new Point
            {
                X = x,
                Y = y,
                Z = z
            };
            cloud.Points.Add(p);
        }
        return cloud;
    }

    internal Matrix4x4 float2matrix(Protoicp.Transform transform_vals)
    {   Matrix4x4 transform = Matrix4x4.zero;
        var i = 0;
        for (var r = 0; r < 4; r++)
        { 
            for(var c = 0; c < 4; c++)
            {
                transform[r,c] = transform_vals.Vals[i];
                i++;
            }

        }
        return transform;
    }

    internal Matrix4x4 getTransform(List<Vector3> new_world_pts, List<Vector3> old_world_pts) 
    {

        Cloud cloud_old = new Cloud
        {
            Points = {}
        };

        Cloud cloud_new = new Cloud
        {
            Points = {}
        };

        cloud_new = vector2cloud(new_world_pts);
        cloud_old = vector2cloud(old_world_pts);
        
        ObjectClouds objClouds = new ObjectClouds
        {
            Clouds = {cloud_old, cloud_new}
        };

        var res = _client.getTransform(objClouds);
        // Debug.Log("Client is currently running");
        // Debug.Log(res);
        var res_mat = float2matrix(res);

        return res_mat;
    }

    private void OnDisable() {
        _channel.ShutdownAsync().Wait();
    }
}