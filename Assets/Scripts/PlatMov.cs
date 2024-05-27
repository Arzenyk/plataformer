using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatMov : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private Vector3 target;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == pointB.position)
        {
            target = pointA.position;
        }
        else if (transform.position == pointA.position)
        {
            target = pointB.position;
        }
    }
}
