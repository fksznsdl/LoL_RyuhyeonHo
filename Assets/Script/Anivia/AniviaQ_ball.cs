using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniviaQ_ball : MonoBehaviour
{
    [SerializeField]
    private AniviaQ aniviaQ;
    [SerializeField]
    private PhotonView pv;
    // Start is called before the first frame update
   
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
                    other.GetComponent<Status>().Damage(aniviaQ.damage, aniviaQ.Anivia.transform.name, Status.AttackType.AP, aniviaQ.Anivia.GetComponent<Status>().APperPenetration, aniviaQ.Anivia.GetComponent<Status>().APpenetration);
                    other.GetComponent<Status>().HitDebuff("µ¿»ó", aniviaQ.slowTime, (int)Debuff.DebuffType.SLOW);
                    other.GetComponent<Status>().isHitAniviaQR = true;
                }
            }
        }
    }
}
