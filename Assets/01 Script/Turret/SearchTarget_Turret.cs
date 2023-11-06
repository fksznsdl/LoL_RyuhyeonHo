using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTarget_Turret : MonoBehaviour
{
    private PhotonView pv;
    private Turret_AI ai;
    private float sightRange;
    private void OnEnable()
    {
        pv = GetComponentInParent<PhotonView>();
        if (!pv.IsMine)
            this.enabled = false;
        ai = this.transform.GetComponentInParent<Turret_AI>();
        sightRange = ai.sightRange;
        this.transform.localScale = new Vector3(sightRange / 2f, sightRange / 2f, 1f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Champion" || other.transform.tag == "Minion")
        {
            if (other.GetComponent<Status>().team != this.transform.GetComponentInParent<Status>().team)
            {
                this.transform.GetComponentInParent<Turret_AI>().GetTarget(other);
                this.gameObject.SetActive(false);
            }
        }
    }/*
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Champion" || other.transform.tag == "Minion")
        {
            if (other.GetComponent<Status>().team != this.transform.GetComponentInParent<Status>().team)
            {
                if(other.transform.tag == "Minion")
                {
                    if(Vector3.Distance(this.transform.GetComponentInParent<Turret_AI>().target.transform.position,this.transform.position)>
                        Vector3.Distance(other.transform.position, this.transform.position))
                    {
                        this.transform.GetComponentInParent<Turret_AI>().GetTarget(other);
                        StartCoroutine(StayCoroutine());
                    }
                }
            }
        }
    }
    private IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }*/
}
