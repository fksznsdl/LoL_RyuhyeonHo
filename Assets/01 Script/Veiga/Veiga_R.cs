using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veiga_R : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Veiga;
    private RaycastHit hitInfo;
    private Vector3 hitpoint;
    private float damage;
    private float currentHpPer;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
        }
        Veiga = GameObject.Find("Veiga");
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, transform.transform.position.z);
        hitInfo = Veiga.GetComponent<Controller>().HitInfo;
        hitpoint = (hitInfo.point - this.transform.position) * Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + hitpoint.x, this.transform.position.y, this.transform.position.z + hitpoint.z);
        if (Vector3.Distance(this.transform.position, hitInfo.point) <= 2f)
        {
            Attack();
        }
    }
    private void Attack()
    {
        currentHpPer = hitInfo.transform.GetComponent<Status>().currentHp / hitInfo.transform.GetComponent<Status>().maxHp;
        if (currentHpPer <= 0.33f) {
            damage = (Veiga.GetComponent<Status>().spell * 1.5f) + (Veiga.GetComponent<Status>().skillsLevel[4] * 150f + 200f);
         }
        else
        {
            damage = (Veiga.GetComponent<Status>().spell * (0.75f * (0.66f / currentHpPer))) + (((Veiga.GetComponent<Status>().skillsLevel[4] * 75f) + 100f) * (0.66f / currentHpPer));
        }
        hitInfo.transform.GetComponent<Status>().Damage(damage, Veiga.transform.name, Status.AttackType.AP, Veiga.GetComponent<Status>().APperPenetration, Veiga.GetComponent<Status>().APpenetration);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
