using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniviaQ_Explosion : MonoBehaviour
{
    [SerializeField]
    private AniviaQ aniviaQ;
    [SerializeField]
    private PhotonView pv;
    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false; return;
        }
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (aniviaQ.status.team != other.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().Damage(aniviaQ.damage, aniviaQ.Anivia.transform.name, Status.AttackType.AP, aniviaQ.status.APperPenetration, aniviaQ.status.APpenetration);
                    other.GetComponent<Status>().HitDebuff("µ¿»ó", aniviaQ.CCTime, (int)Debuff.DebuffType.CC);

                    other.GetComponent<Status>().isHitAniviaQR = true;
                }
            }
        }
    }

}
