using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GilbertWorld : MonoBehaviour
{
    public float permeability = 0.05f;
    public float maxForce = 1000.0f;

    /* calculates the force between two magnetic poles: https://en.wikipedia.org/wiki/Force_between_magnets#Gilbert_Model
    // F = [(q*m1 * q*m2 * permeability) / (4 * PI * r)], where:
    // r = separation
    // qm1, qm2 = magnitudes (TODO: look into this further; currently defined as one term)
    // permeability depends on the environment: https://en.wikipedia.org/wiki/Permeability_(electromagnetism)
    // we all know what PI is... :) */
    Vector3 CalculateGilbertForce(Magnet magnet1, Magnet magnet2)
    {
        Vector3 pos1 = magnet1.transform.position;
        Vector3 pos2 = magnet2.transform.position;

        Vector3 r = pos2 - pos1;
        float term2 = 4 * Mathf.PI * r.magnitude;

        float term1 = magnet1.magnitude * magnet2.magnitude * permeability;

        var F = (term1 / term2);

        // same charge => attraction
        if (magnet1.charge == magnet2.charge)
        {
            F = -F;
        }

        return F * r.normalized;
    }

    void FixedUpdate()
    {
        Magnet[] magnets = FindObjectsOfType<Magnet>();
        int magnetNumber = magnets.Length;

        for (int i = 0; i < magnetNumber; i++)
        {
            Vector3 a1 = Vector3.zero; // aceleration

            Magnet m1 = magnets[i];

            // checks the interaction of each magnet w/ each magnet (TODO: optimize?)
            for (int j = 0; j < magnetNumber; j++)
            {
                // if same magnetic pole
                if (i == j)
                {
                    continue;
                }

                var m2 = magnets[j];

                var F = CalculateGilbertForce(m1, m2);
                var magnitude = m1.magnitude * m2.magnitude;

                a1 += F * magnitude;
            }

            if (a1.magnitude > maxForce)
            {
                a1 = a1.normalized * maxForce;
            }

            var rb1 = m1.rb;
            rb1.AddForceAtPosition(a1, m1.transform.position);
        }
    }
}
