using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float range;

    private Rigidbody rb;
    private Vector3 birthPlace;

    private void Start()
    {
        birthPlace = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        if(Vector3.Distance(birthPlace, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
