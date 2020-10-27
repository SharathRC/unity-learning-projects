using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane;
    private Vector3 pos_offset = new Vector3(45, 0, 0);
    // private Vector3 rot_offset = new Vector3(0, -90, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = plane.transform.position + pos_offset;
        // transform.rotation = plane.transform.rotation + rot_offset;
        // transform.rotation.x = 0;
    }
}
