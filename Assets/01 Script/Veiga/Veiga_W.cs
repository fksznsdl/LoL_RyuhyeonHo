using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Veiga_W : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Veiga;
    [SerializeField]
    private GameObject veigaW;

    private float damage;
    private float downspeed;
    private float currentDelay;

    public RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            GetComponentInChildren<SphereCollider>().enabled = false;
            this.enabled = false;
        }
        Veiga = GameObject.Find("Veiga");
        damage = (Veiga.GetComponent<Status>().spell * 1f) + (Veiga.GetComponent<Status>().skillsLevel[2] * 50f) + 50f;
        this.transform.position = new Vector3(hitInfo.point.x, 0.1f, hitInfo.point.z);
        veigaW.transform.position = new Vector3(hitInfo.point.x, 50f, hitInfo.point.z);
        downspeed = 50f * Time.deltaTime * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        if (currentDelay >=1.2f) {
            veigaW.transform.position = new Vector3(veigaW.transform.position.x, veigaW.transform.position.y - downspeed, veigaW.transform.position.z);
            if (veigaW.transform.position.y <= -10f)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
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
            if(other.GetComponent<Status>().team != Veiga.GetComponent<Status>().team)
            {
                other.GetComponent<Status>().Damage(damage, Veiga.transform.name, Status.AttackType.AP, Veiga.GetComponent<Status>().APperPenetration, Veiga.GetComponent<Status>().APpenetration);
            }
        }
    }
}
