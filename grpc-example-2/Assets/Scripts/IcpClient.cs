using Grpc.Core;
using Protoicp;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IcpClient : MonoBehaviour{
    // public static List<string> items = new List<string>();
    
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

    internal string getTransform(List<Vector3> new_world_pts, List<Vector3> old_world_pts) 
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

        return "test";
    }

    private void OnDisable() {
        _channel.ShutdownAsync().Wait();
    }
}