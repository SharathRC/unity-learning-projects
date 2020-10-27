using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    private float speed = 10;
    private float vechicalRotateSpeed = 20;
    private float horizontalInput = 1;
    private float verticalInput = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        // move the truck (every frame)
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        if (verticalInput!=0)
        {
            transform.Rotate(Vector3.up * vechicalRotateSpeed * Time.deltaTime * horizontalInput * Convert.ToSingle(Math.Round(verticalInput)));
        }

    }
}
