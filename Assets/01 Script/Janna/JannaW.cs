using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannaW : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Janna;
    private float damage;
    private float currentTime;

    private string debuffName;
    private float debuffTime;
    private float slowper;

    private Vector3 originPos;
    private Vector3 targetPos;

    private RaycastHit hitInfo;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
            return;
        }
        Janna = GameObject.Find("Janna");
        this.transform.forward = Janna.GetComponent<Janna_Controller>().chTransform.forward;
        originPos = this.transform.position;
        targetPos = Janna.GetComponent<Janna_Controller>().HitInfo.transform.position;
        debuffName = "¼­Ç³";
        debuffTime = 3f;
        damage = 50f + (30f * Janna.GetComponent<Status>().skillsLevel[2]) + (0.6f * Janna.GetComponent<Status>().spell);
        currentTime = 0f;
        hitInfo = Janna.GetComponent<Janna_Controller>().HitInfo;
        slowper = 16f + (Janna.GetComponent<Status>().skillsLevel[2] * 4f) + (0.06f * Janna.GetComponent<Status>().spell);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        this.transform.position = Vector3.Lerp(originPos, targetPos, currentTime);
        if (Vector3.Distance(this.transform.position, targetPos) <= 1f&&pv.IsMine)
        {
            hitInfo.transform.GetComponent<Status>().Damage(damage,Janna.transform.name,Status.AttackType.AP, Janna.GetComponent<Status>().APperPenetration, Janna.GetComponent<Status>().APpenetration);
            hitInfo.transform.GetComponent<Status>().HitDebuff(debuffName, debuffTime, slowper,(int)Debuff.DebuffType.SLOW);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
