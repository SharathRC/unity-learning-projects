using Grpc.Core;
using Protoicp;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IcpClient {
    // public static List<string> items = new List<string>();
    
    private readonly CalcTransform.CalcTransformClient _client;
    private readonly Channel _channel;
    // private readonly string _server = "127.0.0.1:50051";
    private readonly string _server = "localhost:50051";

    internal IcpClient() {
        _channel = new Channel(_server, ChannelCredentials.Insecure);
        _client = new CalcTransform.CalcTransformClient(_channel);
    }

    internal string getTransform() {
        Point pt1 = new Point
        {
            X = 0.0f,
            Y = 0.0f,
            Z = 0.0f
        };
        
        Point pt2 = new Point
        {
            X = 1.0f,
            Y = 0.0f,
            Z = 0.0f
        };
        
        Point pt3 = new Point        
        {
            X = 0.0f,
            Y = 1.0f,
            Z = 0.0f
        };
        
        Point pt4 = new Point
        {
            X = 0.0f,
            Y = 0.0f,
            Z = 1.0f
        };
        
        Point pt5 = new Point        
        {
            X = 1.0f,
            Y = 1.0f,
            Z = 1.0f
        };
        
        Point pt6 = new Point        
        {
            X = 2.0f,
            Y = 0.0f,
            Z = 0.0f
        };
        
        Point pt7 = new Point        
        {
            X = 0.0f,
            Y = 2.0f,
            Z = 0.0f
        };
        
        Point pt8 = new Point        
        {
            X = 0.0f,
            Y = 0.0f,
            Z = 2.0f
        };
        
        Google.Protobuf.Collections.RepeatedField<Point> pts = new Google.Protobuf.Collections.RepeatedField<Point>();
        pts.Add(pt1);
        pts.Add(pt2);

        Cloud cloud1 = new Cloud
        {
            Points = {}
        };

        cloud1.Points.Add(pt1);
        cloud1.Points.Add(pt2);
        cloud1.Points.Add(pt3);
        cloud1.Points.Add(pt4);

        Cloud cloud2 = new Cloud
        {
            Points = { pt5, pt6, pt7, pt8 }
        };
        
        Cloud cloud3 = new Cloud
        {
            // Points = {}
        };

        ObjectClouds objClouds = new ObjectClouds
        {
            Clouds = {cloud2, cloud1}
        };

        var res = _client.getTransform(objClouds);
        Debug.Log("Client is currently running");
        Debug.Log(res);

        return "test";
    }

    private void OnDisable() {
        _channel.ShutdownAsync().Wait();
    }
}