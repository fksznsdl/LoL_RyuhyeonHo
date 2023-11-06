using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public Status status;
    // Start is called before the first frame update
    void OnEnable()
    {
        status = this.transform.parent.GetComponentInParent<Status>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Environment" || other.transform.tag == "Building")
        {
            status.isNearWall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Environment" || other.transform.tag == "Building")
        {
            status.isNearWall = false;
        }
    }
}
