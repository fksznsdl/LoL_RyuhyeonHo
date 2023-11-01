using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxE_Slow : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;
    [SerializeField]
    private LuxE LuxE;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false; return;
        }
        else if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != LuxE.Lux.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().currentMoveSpeed *= (1 - (LuxE.slowper / 100f));
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != LuxE.Lux.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().ResetSlow();
                }
            }
        }
    }

}
