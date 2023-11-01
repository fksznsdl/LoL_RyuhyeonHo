using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lux_Attack : Attack
{
    private GameObject Lux;
    private float spelldamage;
    private bool isAttack;

    protected override void Start()
    {
        base.Start();
        if(!pv.IsMine)
        {
            this.enabled = false;
        }
        if (pv.IsMine)
        {
            Lux = GameObject.Find("Lux");
            hitInfo = Lux.GetComponent<Lux_Controller>().HitInfo;
            targetPos = Lux.GetComponent<Lux_Controller>().HitInfo.transform.position;
            currentTime = 0f;
            spelldamage = 0f;
            status = Lux.GetComponent<Status>();
            isAttack = false;
        }
    }

    protected override void Move()
    {
        targetPos = hitInfo.transform.position;
        this.transform.position = Vector3.Lerp(orginPos, targetPos, currentTime);
        if (Vector3.Distance(this.transform.position, targetPos) <= 1f && pv.IsMine && !isAttack)
        {
            isAttack = true;
            if (hitInfo.transform.tag == "Bullding" && Lux.GetComponent<Status>().spell > Lux.GetComponent<Status>().attackDamage)
            {
                spelldamage = Lux.GetComponent<Status>().spell * 0.6f;
                hitInfo.transform.GetComponent<Status>().Damage(spelldamage, Lux.transform.name, Status.AttackType.AP, status.APperPenetration, status.APpenetration);
            }
            hitInfo.transform.GetComponent<Status>().Damage(Lux.GetComponent<Status>().attackDamage, Lux.transform.name,Status.AttackType.AD,status.ADperPenetration,status.ADpenetration);
            if(hitInfo.transform.GetComponent<Status>().isDead)
            {
                PhotonNetwork.Destroy(this.gameObject);
                return;
            }
            else if (hitInfo.transform.GetComponent<Status>().currentHp >= 0f)
            {
                Lux.GetComponent<Lux_Controller>().HitPassive(hitInfo);
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
