using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anivia_Attack : Attack
{ 
    private GameObject Anivia;
    private float spelldamage;

    protected override void Start()
    {
        base.Start();
        if (pv.IsMine)
        {
            Anivia = GameObject.Find("Anivia");
            hitInfo = Anivia.GetComponent<Anivia_Controller>().HitInfo;
            targetPos = Anivia.GetComponent<Anivia_Controller>().HitInfo.transform.position;
            currentTime = 0f;
            spelldamage = 0f;
            status = Anivia.GetComponent<Status>();
        }
    }

    protected override void Move()
    {
        if (pv.IsMine)
        {
            targetPos = hitInfo.transform.position;
            this.transform.position = Vector3.Lerp(orginPos, targetPos, currentTime);
            if (Vector3.Distance(this.transform.position, targetPos) <= 1f)
            {
                if (hitInfo.transform.tag == "Bullding" && status.spell > status.attackDamage)
                {
                    spelldamage = Anivia.GetComponent<Status>().spell * 0.6f;
                    hitInfo.transform.GetComponent<Status>().Damage(spelldamage, Anivia.transform.name, Status.AttackType.AP, status.APperPenetration, status.APpenetration);
                }
                hitInfo.transform.GetComponent<Status>().Damage(Anivia.GetComponent<Status>().attackDamage, Anivia.transform.name, Status.AttackType.AD, status.ADperPenetration, status.ADpenetration);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
