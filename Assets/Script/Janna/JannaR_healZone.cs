using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannaR_healZone : MonoBehaviour
{
    private JannaR JannaR;
    private float currnetTime;
    private PhotonView pv;
    // Start is called before the first frame update
    void OnEnable()
    {
        pv = GetComponentInParent<PhotonView>();
        JannaR = GetComponentInParent<JannaR>();
        currnetTime = 0f;
        if (pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false;
            this.enabled = false;
            return;
        }
    }
    private void Update()
    {
        currnetTime += Time.deltaTime;
        if (currnetTime >= 0.1f)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion")
            {
                if (other.GetComponent<Status>().team == JannaR.Janna.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().Healing(JannaR.heal);
                }
            }
        }
    }
}
