using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using MathNet.Numerics.LinearAlgebra;
// using MathNet.Numerics.LinearAlgebra.Double;

public class Utilities : MonoBehaviour
{
    internal Matrix4x4 world2camera()
    {   
        Matrix4x4 world_T_camera = Matrix4x4.TRS(Camera.main.transform.position, Camera.main.transform.rotation, new Vector3(1, 1, 1));
        // Debug.Log("Camera transformation matrix: " + Camera.main.worldToCameraMatrix);
        // Debug.Log("matrix: " + world_T_camera);
        return world_T_camera;

    }

    internal Vector3[] transform_points(Vector3[] org_vertices, Matrix4x4 custom_transform)
    {   
        Vector3[] new_vertices = new Vector3[org_vertices.Length];
        for (var i = 0; i < org_vertices.Length; i++)
        {
            var pt = custom_transform.MultiplyPoint3x4(org_vertices[i]);
            new_vertices[i] = pt;
        }

        return new_vertices;
    }

    internal Vector3 GetVertexWorldPosition(Vector3 vertex, Transform owner)
    {
        return owner.localToWorldMatrix.MultiplyPoint3x4(vertex);
    }

}
