using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxQ : MonoBehaviour
{
    private PhotonView pv;
    [SerializeField]
    private BoxCollider col;
    private GameObject Lux;
    private float damage;
    private int count;
    private Vector3 orginPos;
    private Vector3 destination;
    private float currentTime;

    private string debuffName;
    private float CCTime;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            col.enabled = false;
            this.enabled = false; return;
        }
        Lux = GameObject.Find("Lux");
        damage = 35f + (Lux.GetComponent<Status>().skillsLevel[1] * 45f) + (Lux.GetComponent<Status>().spell * 0.6f);
        count = 2;
        orginPos = this.transform.position;
        destination = Lux.GetComponent<Lux_Controller>().chTransform.forward * Lux.GetComponent<Lux_Controller>().LuxQRange + orginPos;
        currentTime = 0f;
        debuffName = "ºûÀÇ ¼Ó¹Ú";
        CCTime = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * 0.75f;
        this.transform.position = Vector3.Lerp(orginPos, destination, currentTime);
        if(Vector3.Distance(this.transform.position,destination) <=0.1f || count <= 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != Lux.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().Damage(damage, Lux.transform.name, Status.AttackType.AP, Lux.GetComponent<Status>().APperPenetration, Lux.GetComponent<Status>().APpenetration);
                    other.GetComponent<Status>().HitDebuff(debuffName, CCTime, 100f, (int)Debuff.DebuffType.RESTRICTION);
                    Lux.GetComponent<Lux_Controller>().Passive(other);
                    count--;
                }
            }
    }
}
