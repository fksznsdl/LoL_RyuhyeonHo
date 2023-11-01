using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTarget : MonoBehaviour
{
    private PhotonView pv;
    private Turret_AI ai;
    private float sightRange;
    private void OnEnable()
    {
        pv = GetComponentInParent<PhotonView>();
        if(!pv.IsMine)
            this.enabled = false;
        ai = this.transform.GetComponentInParent<Turret_AI>();
        sightRange = ai.sightRange;
        this.transform.localScale = new Vector3(sightRange / 2f, sightRange / 2f, 1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Champion")
        {
            if (other.GetComponent<Status>().team != ai.status.team)
            {
                if (other.GetComponent<Status>().isChampAttack)
                {
                    ai.ChangeTarget(other);
                }
            }
        }
        else if (ai.target != null) {
            if (other.transform.tag == "Minion" && ai.target.transform.tag == "Minion")
            {
                if (other.GetComponent<Status>().minionType == "Warrior" && ai.target.GetComponent<Status>().minionType == "Wizard")
                {
                    if (other.GetComponent<Status>().team != ai.status.team)
                    {
                        ai.ChangeTarget(other);
                    }
                }
            }
        }
    }
}
