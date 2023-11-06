using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionWizard_Attack : MonoBehaviour
{
    private PhotonView pv;
    public Minion_AI minion;
    private Vector3 originPos;
    private Vector3 targetPos;
    private float currentTime;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
            return;
        }
        originPos = this.transform.position;
        if (minion.target == null)
        {
            PhotonNetwork.Destroy(this.gameObject);
            return;
        }
        targetPos = minion.target.transform.position;
        currentTime = 0f;
    }

    private void Update()
    {
        if (minion.target == null)
        {
            PhotonNetwork.Destroy(this.gameObject);
            return;
        }
        targetPos = minion.target.transform.position;
        currentTime += Time.deltaTime;
        this.transform.position = Vector3.Lerp(originPos, targetPos, currentTime);
        if (Vector3.Distance(this.transform.position, targetPos) <= 0.1f)
        {
            minion.target.GetComponent<Status>().Damage(minion.status.attackDamage);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

}