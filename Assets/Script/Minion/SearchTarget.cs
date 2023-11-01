using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTarget : MonoBehaviour
{
    private Minion_AI ai;
    private float sightRange;
    private void OnEnable()
    {
        ai = this.transform.GetComponentInParent<Minion_AI>();
        sightRange = ai.sightRange;
        this.transform.localScale = new Vector3(sightRange / 2f, sightRange/2f, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Building")
        {
            if(other.GetComponent<Status>().team != this.transform.GetComponentInParent<Status>().team)
            {
                this.transform.GetComponentInParent<Minion_AI>().GetTarget(other);
            }
        }
    }
}
