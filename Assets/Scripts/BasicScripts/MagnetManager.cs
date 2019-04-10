using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    private float updateInterval = 0.01f;
    private List<Magnet> magnets;
    private List<MovingMagnet> movingMagnets;

    // start is called before the first frame update
    void Start()
    {
        magnets = new List<Magnet>(FindObjectsOfType<Magnet>());
        movingMagnets = new List<MovingMagnet>(FindObjectsOfType<MovingMagnet>());

        foreach(MovingMagnet mm in movingMagnets)
        {
            StartCoroutine(UpdateForce(mm));
        }
    }

    // update the force for each non fixed magnet (aka "moving magnet") every "updateInterval" seconds
    public IEnumerator UpdateForce(MovingMagnet mm)
    {
        bool isFirst = true;

        while (true)
        {
            if (isFirst)
            {
                isFirst = false;
                yield return new WaitForSeconds(Random.Range(0.0f, updateInterval));
            }

            ApplyMagneticForce(mm);
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void ApplyMagneticForce(MovingMagnet mm)
    {
        Vector3 newForce = Vector3.zero;

        foreach (Magnet m in magnets)
        {
            // if it's the same magnet
            if (mm == m)
            {
                continue;
            }

            float distance = Vector3.Distance(mm.transform.position, m.transform.position);
            float force = 512 * mm.charge * m.charge / Mathf.Pow(distance, 2);

            Vector3 direction = mm.transform.position - m.transform.position;
            direction.Normalize();

            newForce += force * direction * updateInterval;

            // handle 0 division
            if (float.IsNaN(newForce.x))
            {
                newForce = Vector3.zero;
            }

            mm.rb.AddForce(newForce);
        }
    }
}
