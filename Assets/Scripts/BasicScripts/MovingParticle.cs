using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParticle : Particle
{
    public float mass = 1.0f;
    public Rigidbody rb;

    // start is called before the first frame update
    void Start()
    {
        UpdateColor();
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.useGravity = false;
    }
}
