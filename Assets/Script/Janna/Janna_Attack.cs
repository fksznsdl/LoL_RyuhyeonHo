using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janna_Attack : Attack
{
    private GameObject Janna;
    private float spelldamage;

    protected override void Start()
    {
        base.Start();
        if (pv.IsMine)
        {
            Janna = GameObject.Find("Janna");
            hitInfo = Janna.GetComponent<Janna_Controller>().HitInfo;
            targetPos = Janna.GetComponent<Janna_Controller>().HitInfo.transform.position;
            currentTime = 0f;
            spelldamage = 0f;
            status = Janna.GetComponent<Status>();
        }
    }

    protected override void Move()
    {
        targetPos = hitInfo.transform.position;
        this.transform.position = Vector3.Lerp(orginPos, targetPos, currentTime);
        if (Vector3.Distance(this.transform.position, targetPos) <= 1f && pv.IsMine)
        {
            if (hitInfo.transform.tag == "Bullding" && status.spell > status.attackDamage)
            {
                spelldamage = Janna.GetComponent<Status>().spell * 0.6f;
                hitInfo.transform.GetComponent<Status>().Damage(spelldamage, Janna.transform.name, Status.AttackType.AP, status.APperPenetration, status.APpenetration);
            }
            hitInfo.transform.GetComponent<Status>().Damage(Janna.GetComponent<Status>().attackDamage, Janna.transform.name, Status.AttackType.AD, status.ADperPenetration, status.ADpenetration);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
