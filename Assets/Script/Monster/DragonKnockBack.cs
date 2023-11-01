using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonKnockBack : MonoBehaviour
{
    private Transform orginPos;
    private float force;

    private void OnEnable()
    {
        orginPos = GetComponentInParent<Transform>();
        force = 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Champion")
        {
            other.GetComponent<Status>().KnockBack(force - Vector3.Distance(orginPos.position,other.transform.position),orginPos.position);
        }
    }


}
