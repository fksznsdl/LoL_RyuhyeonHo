using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veiga_Attack : Attack
{
    private GameObject Veiga;
    private float spelldamage;

    protected override void Start()
    {
        base.Start();
        if (pv.IsMine)
        {
            Veiga = GameObject.Find("Veiga");
            hitInfo = Veiga.GetComponent<Veiga_Controller>().HitInfo;
            targetPos = Veiga.GetComponent<Veiga_Controller>().HitInfo.transform.position;
            currentTime = 0f;
            spelldamage = 0f;
            status = Veiga.GetComponent<Status>();
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
                spelldamage = Veiga.GetComponent<Status>().spell * 0.6f;
                hitInfo.transform.GetComponent<Status>().Damage(spelldamage, Veiga.transform.name, Status.AttackType.AP, status.APperPenetration, status.APpenetration);
            }
            hitInfo.transform.GetComponent<Status>().Damage(Veiga.GetComponent<Status>().attackDamage, Veiga.transform.name, Status.AttackType.AD, status.ADperPenetration, status.ADpenetration);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
