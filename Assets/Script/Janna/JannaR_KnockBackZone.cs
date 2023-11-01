using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannaR_KnockBackZone : MonoBehaviour
{
    [SerializeField]
    private JannaR jannaR;
    private PhotonView pv;
    private void OnEnable()
    {
        pv = GetComponentInParent<PhotonView>();
        if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false;
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.transform.tag == "Minion" || other.transform.tag == "Champion" || other.transform.tag == "Monster")
            {
                if (jannaR.Janna.GetComponent<Status>().team != other.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().RPCKnockBack(jannaR.KnockBackForce - Vector3.Distance(other.transform.position, jannaR.Janna.transform.position), jannaR.Janna.transform.position);
                }
            }
        }
    }

}
