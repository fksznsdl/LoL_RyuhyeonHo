using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    private PhotonView pv;
    public Turret_AI turret_AI;
    private Vector3 originPos;
    private Vector3 targetPos;
    private float currentTime;
    private Collider target;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false; return;
        }
        originPos = this.transform.position;
        if(turret_AI == null || turret_AI.target==null)
        {
            if (pv.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
                return;
            }
        }
        targetPos = turret_AI.target.transform.position;
        target = turret_AI.target;
    }
    void Update()
    {
        if(target.GetComponent<Status>().isDead || target == null)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        targetPos = target.transform.position;
        currentTime += Time.deltaTime;
        this.transform.position = Vector3.Lerp(originPos, targetPos, currentTime);
        if (Vector3.Distance(this.transform.position, targetPos) <= 1f)
        {
            if(target.transform.tag == "Minion")
            {
                switch (target.GetComponent<Status>().minionType)
                {
                    case "Warrior":
                        target.GetComponent<Status>().Damage(target.GetComponent<Status>().maxHp * 0.45f);
                        PhotonNetwork.Destroy(this.gameObject);
                        return;
                    case "Wizard":
                        target.GetComponent<Status>().Damage(target.GetComponent<Status>().maxHp * 0.70f);
                        PhotonNetwork.Destroy(this.gameObject);
                        return;
                }
            }
            target.GetComponent<Status>().Damage(turret_AI.status.attackDamage);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
