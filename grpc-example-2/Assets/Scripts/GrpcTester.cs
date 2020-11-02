using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrpcTester : MonoBehaviour
{
    // private ColorClient _colorClient;
    private IcpClient _icpClient;

    // Start is called before the first frame update
    void Start()
    {
        // _colorClient = new ColorClient();
        _icpClient = new IcpClient();
        
    }

    // Update is called once per frame
    void Update()
    {
        // var newColorString = _colorClient.GetRandomColor("oldcolor");
        // print(newColorString);
        var res = _icpClient.getTransform();
        // print(res);
        
    }
}
