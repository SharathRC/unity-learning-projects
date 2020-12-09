using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using MathNet.Numerics.LinearAlgebra;
// using MathNet.Numerics.LinearAlgebra.Double;

public class Utilities : MonoBehaviour
{   int[,] array = new int[4, 2];
    public Vector3 offset = new Vector3(0, 1, 0);
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    internal void camera2world()
    {   
        // Camera.main.transform.Rotate(90, 0, 0, Space.Self);
        // Camera.main.transform.Rotate(90, 0, 0, Space.World);
        // print(Camera.main.transform.position);
        // print(Camera.main.transform.rotation);

        // print("x local angle: " + Camera.main.transform.localEulerAngles.x);
        // print("Y local angle: " + Camera.main.transform.localEulerAngles.y);
        // print("z local angle: " + Camera.main.transform.localEulerAngles.z);

        // print("x world angle: " + Camera.main.transform.rotation.x);
        // print("Y world angle: " + Camera.main.transform.rotation.y);
        // print("z world angle: " + Camera.main.transform.rotation.z);

        Vector3 camoffset = new Vector3(0, 0, 0);
        Matrix4x4 m = Matrix4x4.TRS(camoffset, Quaternion.identity, new Vector3(1, 1, 1));
        // Camera.main.worldToCameraMatrix = m;

        Debug.Log("Camera transformation matrix: " + Camera.main.worldToCameraMatrix);

    }
}
