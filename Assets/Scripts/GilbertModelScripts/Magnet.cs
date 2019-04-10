using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public enum Charge
    {
        neg, pos
    }

    public Charge charge;
    public Rigidbody rb;
    public float magnitude;
}
