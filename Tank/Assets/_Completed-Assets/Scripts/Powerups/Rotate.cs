using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 3f;
    public float xaxis = 0;
    public float yaxis = 0;
    public float zaxis = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(xaxis, yaxis, zaxis) * speed);
    }
}
