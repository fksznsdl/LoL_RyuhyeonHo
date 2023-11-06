using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetExpZone : MonoBehaviour
{
    private PhotonView pv;
    private Status status;
    [SerializeField]
    private SphereCollider col;

    private List<Collider> ExceptOthers;

    private float currentTime;
    private void Start()
    {
        pv = GetComponentInParent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
            return;
        }
        else {
            status = GetComponentInParent<Status>();
            ExceptOthers = new List<Collider>();
            currentTime = 0f;
            col.enabled = true;
         }
    }
    private void Update()
    {
        if (currentTime > 0f)
        {
            currentTime-=Time.deltaTime;
            if(currentTime <= 0f)
            {
                ExceptOthers.Clear();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != status.team)
                {
                    if (other.GetComponent<Status>().isDead)
                    {
                        for (int i = 0; i < ExceptOthers.Count; i++)
                        {
                            if (other == ExceptOthers[i])
                            {
                                return;
                            }
                        }
                        status.GetExp(other.GetComponent<Status>().killExp);
                        currentTime = 3.5f;
                        ExceptOthers.Add(other);
                    }
                }
            }
        }
    }
}
