using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonKnockBack : MonoBehaviour
{
    private float force;
    private PhotonView pv;
    private void OnEnable()
    {
        force = 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.transform.tag == "Champion")
            {
                other.GetComponent<Status>().RPCKnockBack(force - Vector3.Distance(this.transform.position, other.transform.position), this.transform.position);
            }
    }


}
