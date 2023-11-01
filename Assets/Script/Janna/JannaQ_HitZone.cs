using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannaQ_HitZone : MonoBehaviour
{
    [SerializeField]
    private JannaQ jannaQ;
    [SerializeField]
    private PhotonView pv;
    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false;
        }
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != jannaQ.Janna.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().Damage(jannaQ.damage, jannaQ.Janna.transform.name, Status.AttackType.AP, jannaQ.Janna.GetComponent<Status>().APperPenetration, jannaQ.Janna.GetComponent<Status>().APpenetration);
                    other.GetComponent<Status>().HitDebuff(jannaQ.debuffName, jannaQ.CCTime, jannaQ.debuffType);
                }
            }
        }
    }
}
