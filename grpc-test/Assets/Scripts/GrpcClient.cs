using Grpc.Core;
using Protomsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrpcClient : MonoBehaviour
{   
    private readonly Tester.TesterClient _client;
    private readonly Channel _channel;
    private readonly string _server = "localhost:50051";
    // Start is called before the first frame update

    GrpcClient() 
    {   
        _channel = new Channel(_server, ChannelCredentials.Insecure);
        _client = new Tester.TesterClient(_channel);
    }

    private int test_grpc() 
    {   
        print("here");
        var recieved_data = _client.test_grpc(new TestMsg {X = 1});
        Debug.Log("return value: " + recieved_data.X);
        print(recieved_data);
        return recieved_data.X;
    }

    private void OnDisable() {
        _channel.ShutdownAsync().Wait();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // print("here");
        var x = test_grpc();
        print(x);
    }
}
