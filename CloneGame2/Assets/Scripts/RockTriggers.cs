using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTriggers : MonoBehaviour
{
    [Header("Section Controls")]
    public GameObject thisSectionRocks;    // Rocks for THIS section
    public GameObject previousSectionRocks; // Rocks for PREVIOUS section (to turn off)
    public GameObject nextSectionRocks;     // Rocks for NEXT section (to turn off)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Turn ON rocks for this section
            if (thisSectionRocks != null)
            {
                thisSectionRocks.SetActive(true);
                Debug.Log("Activated Section: " + thisSectionRocks.name);
            }

            // Turn OFF rocks for previous section (if exists)
            if (previousSectionRocks != null)
            {
                previousSectionRocks.SetActive(false);
                Debug.Log("Deactivated Previous Section: " + previousSectionRocks.name);
            }

            // Turn OFF rocks for next section (if exists) - prevents overlap
            if (nextSectionRocks != null)
            {
                nextSectionRocks.SetActive(false);
                Debug.Log("Deactivated Next Section: " + nextSectionRocks.name);
            }
        }
    }
}
