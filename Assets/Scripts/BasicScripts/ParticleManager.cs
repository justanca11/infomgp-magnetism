using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private float updateInterval = 0.01f;
    private Particle[] particles;
    private MovingParticle[] movingParticles;

    // start is called before the first frame update
    void Start()
    {
        particles = FindObjectsOfType<Particle>();
        movingParticles = FindObjectsOfType<MovingParticle>();

        foreach (MovingParticle mp in movingParticles)
        {
            StartCoroutine(UpdateForce(mp));
        }
    }

    // update the force for each non fixed particle every "updateInterval" seconds
    public IEnumerator UpdateForce(MovingParticle mp)
    {
        bool isFirst = true;

        while (true)
        {
            if (isFirst)
            {
                isFirst = false;
                yield return new WaitForSeconds(Random.Range(0.0f, updateInterval));
            }

            ApplyParticleicForce(mp);
            yield return new WaitForSeconds(updateInterval);
        }
    }

    // push / pull particles together inversley proportional to the distance b/w them
    private void ApplyParticleicForce(MovingParticle mp)
    {
        Vector3 F2 = Vector3.zero;

        foreach (Particle p in particles)
        {
            // if it's the same Particle
            if (mp == p)
            {
                continue;
            }
            Vector3 pos1 = mp.transform.position;
            Vector3 pos2 = p.transform.position;

            float distance = (pos1 - pos2).magnitude;
            float F = (512.0f * mp.charge * p.charge) / Mathf.Pow(distance, 2);

            Vector3 direction = (pos1 - pos2).normalized;
            F2 += F * direction * updateInterval;

            // handle NANs caused by 0 division (TODO: improve?)
            if (float.IsNaN(F2.x))
            {
                F2 = Vector3.zero;
            }

            mp.rb.AddForce(F2);
        }
    }
}
