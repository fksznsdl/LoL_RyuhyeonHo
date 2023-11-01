using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veiga_Q : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Veiga;
    private Vector3 movePoint;
    private Vector3 orginPoint;
    private float currentTime;
    private int count;

    private float damage;
    private float speed;
    void Start()
    {
       pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
            
        }
        count = 2;
        speed = 1.5f;
        Veiga = GameObject.Find("Veiga");

        this.transform.position = new Vector3(Veiga.transform.position.x, 1f, Veiga.transform.position.z);
        orginPoint = this.transform.position;
        movePoint = Veiga.GetComponent<Veiga_Controller>().VeigaQRange * Veiga.GetComponent<Controller>().chTransform.forward + orginPoint;
        movePoint = new Vector3(movePoint.x, 1f, movePoint.z);
        damage = Veiga.GetComponent<Status>().skillsLevel[1] * 80 + (0.6f * Veiga.GetComponent<Status>().spell);
    }
    void Update()
    {
        currentTime += Time.deltaTime*speed;
        if (count == 0)
            PhotonNetwork.Destroy(this.gameObject);
        this.transform.position = Vector3.Lerp(orginPoint, movePoint, currentTime);
        if(Vector3.Distance(this.transform.position, movePoint) <= 0.01f)
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
            if (other.GetComponent<Status>().team != Veiga.GetComponent<Status>().team)
            {
                other.GetComponent<Status>().Damage(damage, Veiga.transform.name, Status.AttackType.AP, Veiga.GetComponent<Status>().APperPenetration, Veiga.GetComponent<Status>().APpenetration);
                if(other.transform.tag == "Champion"|| other.GetComponent<Status>().currentHp <= 0)
                {
                    Veiga.GetComponent<Status>().spell += 2;
                }
                count--;
                
            }
        }
    }

}
