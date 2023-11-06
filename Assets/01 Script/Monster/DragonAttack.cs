using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttack : Attack
{
    public Dragon_AI dragon;
    private GameObject target;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!pv.IsMine)
            this.enabled = false;
        target = dragon.GetComponent<Dragon_AI>().target;
        targetPos = target.transform.position;
        currentTime = 0f;
    }
    protected override void Update()
    {
        if (target.GetComponent<Status>().isDead)
        {
            PhotonNetwork.Destroy(this.gameObject);
            return;
        }
        currentTime += Time.deltaTime;
        Move();
    }
    protected override void Move()
    {
        this.transform.position = Vector3.Lerp(orginPos, targetPos, currentTime);
        if (Vector3.Distance(this.transform.position, targetPos) <= 1f)
        {
            target.transform.GetComponent<Status>().Damage(dragon.status.attackDamage);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
